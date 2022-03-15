using ApiRequests.Imagga;
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

namespace DomainModel.Services
{
    public class MediaService : IMediaService
    {
        readonly IUnitOfWork _unitOfWork;

        readonly ImaggaApi _imagga;

        readonly WordAssociationsApi _wordAssociations;

        public MediaService(IDbFactory dbFactory, MyConfigurations configurations)
        {
            _unitOfWork = dbFactory.GetDataAccess();
            
            _imagga = new ImaggaApi(configurations.ImaggaKey.ImaggaApiKey, configurations.ImaggaKey.ImaggaApiSecret);
            
            _wordAssociations = new(configurations.WordAssociationsApiKey);
        }


        public async Task<List<string>> SearchMedia(string keyWord)
        {
            var searchWord = keyWord.ToLower();
            var imgsTemp = await _unitOfWork.MediaSearchRepository.Search(searchWord);
            
            var imags = RootToString(imgsTemp);

            return imags;

            //var result = new List<string>();

            //List<Task<Tuple<string, List<ImaggaTag>>>> tasks = new();
            //foreach(var im in imags)
            //{
            //    tasks.Add(TagImage(im));
            //}
            
            //var imageAndtags = await Task.WhenAll(tasks);
            
            //var reletedWords = await GetWordAssociations(searchWord);
            
            //result.AddRange(from t in imageAndtags
            //                where IsImageReletedToSearchWord(reletedWords, t.Item2)
            //                select t.Item1);

            //// no data from db or firebase => call nasa => call imagga => save relevt data to db
            //return result;
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
                                    //where a.Pos == Pos.Noun
                                    select a.Word.ToLower();
            var inter = tagsWords.Intersect(associationsWords);
            if (inter.Any())
            {
                return  true;
            }

            return false;
        }

        private List<string> RootToString(Models.Dtos.Root imgs)
        {
            var a = imgs.collection.items;
            List<string> result = new List<string>();
            var temp = true;
            foreach (var item in a)
            {
                if (item.links != null)
                    foreach (var link in item.links)
                    {
                        if (link != null)
                        {
                            temp = link.href.ToString().EndsWith(".jpg");
                            if (temp)
                                result.Add(link.href);
                        }
                    }
            }
            return result;
        }
    }
}
