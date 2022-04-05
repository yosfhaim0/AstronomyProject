﻿using ApiRequests.Imagga;
using DataAccess.UnitOfWork;
using DomainModel.DbFactory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using Models.Configurations;
using ApiRequests.WordAssociations;
using ApiRequests.Nasa;
using Tools;

namespace DomainModel.Services
{
    public class MediaService : IMediaService
    {
        readonly IUnitOfWork _unitOfWork;

        readonly ImaggaApi _imagga;

        readonly WordAssociationsApi _wordAssociations;

        readonly NasaApi _nasaApi;

        public MediaService(IDbFactory dbFactory, MyConfigurations configurations)
        {
            _unitOfWork = dbFactory.GetDataAccess(); ;
            
            _imagga = new ImaggaApi(configurations.ImaggaKey.ImaggaApiKey, configurations.ImaggaKey.ImaggaApiSecret);

            _nasaApi = new NasaApi(configurations.CurrentNasaApiKey);

            _wordAssociations = new(configurations.WordAssociationsApiKey);
        }


        public async Task<IEnumerable<MediaGroupe>> SearchMedia(string keyWord)
        {
            keyWord = keyWord.ToLower();
            var medias = await _unitOfWork.MediaSearchRepository.GetAll();
            //await _unitOfWork
            //.MediaSearchRepository
            //.Search(keyWord);

            //if (medias.Any())
            //{
            //    return medias;
            //}

            //var mediasFromNasa = await _nasaApi.SearchMedia(keyWord);

            //foreach(var m in medias)
            //{
            //    await _unitOfWork.MediaSearchRepository.AddSearchWord(m,
            //        new SearchWordModel
            //        {
            //            SearchWord = keyWord
            //        });
            //}
            //await _unitOfWork.Complete();

            var result = await ConfigureMedia(medias);

            if (result.Any())
            {
                await _unitOfWork
                    .MediaSearchRepository
                    .InsertMany(result);
                await _unitOfWork.Complete();
            }

            return result;
        }

        private async Task<IEnumerable<MediaGroupe>> ConfigureMedia(IEnumerable<MediaGroupe> mediasFromNasa)
        {
            List<Task<Tuple<string, List<ImaggaTag>>>> tasks = new();
            foreach (var im in mediasFromNasa.Select(m => m.PreviewUrl))
            {
                tasks.Add(TagImage(im));
            }

            var imageAndTags = await Task.WhenAll(tasks);

            foreach(var m in mediasFromNasa)
            {
                foreach(var imt in imageAndTags)
                {
                    var image = imt.Item1;
                    var tags = imt.Item2;
                    if (m.PreviewUrl == image)
                    {
                        await _unitOfWork.MediaSearchRepository.AddTags(m, tags);
                    }
                }
            }
            return mediasFromNasa;
        }

        private async Task<IEnumerable<MediaGroupe>> ConfigureMediaSmart(IEnumerable<MediaGroupe> mediasFromNasa, string keyWord)
        {
            List<Task<Tuple<string, List<ImaggaTag>>>> tasks = new();
            foreach (var im in mediasFromNasa.Select(m => m.Url))
            {
                tasks.Add(TagImage(im));
            }

            var imageAndTags = await Task.WhenAll(tasks);

            var reletedWords = await GetWordAssociations(keyWord);

            var urls = (from t in imageAndTags
                       where IsImageReletedToSearchWord(reletedWords, t.Item2)
                       select t.Item1).ToList();

            return from u in urls
                      where u != null
                      select new MediaGroupe
                      {

                      };

        }

        private async Task<Tuple<string, List<ImaggaTag>>> TagImage(string imageUrl)
        {

            var imaggTag = await _imagga.AutoTagging(imageUrl);
            var tags = imaggTag.GetTags();

            return new(imageUrl, tags);
        }

        private async Task<List<Association>> GetWordAssociations(string searchWord)
        {
            return await _wordAssociations.GetAssociations(searchWord);
        }

        private bool IsImageReletedToSearchWord(List<Association> associations, List<ImaggaTag> tags)
        {
            if(tags == null)
            {
                return false;
            }

            var tagsWords = tags.Select(t => t.Tag.ToLower());
            var associationsWords = from a in associations
                                    orderby a.Weight descending
                                    //where a.Pos == Pos.Noun
                                    select a.Word.ToLower();
            var inter = tagsWords.Intersect(associationsWords.Take(25));
            if (inter.Any())
            {
                return  true;
            }

            return false;
        }

        
    }
}
