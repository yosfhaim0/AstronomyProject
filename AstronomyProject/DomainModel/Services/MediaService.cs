using ApiRequests.Imagga;
using DomainModel.DataAccessFactory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models;
using Models.Configurations;
using ApiRequests.WordAssociations;
using ApiRequests.Nasa;
using ApiRequests.FireBaseStorage;

namespace DomainModel.Services
{
    public class MediaService : IMediaService
    {
        readonly ImaggaApi _imagga;

        readonly WordAssociationsApi _wordAssociations;

        readonly NasaApi _nasaApi;

        readonly FireBase _fireBase;

        readonly IDataAccessFactory _daFactory;

        public MediaService(IDataAccessFactory daFactory, MyConfigurations configurations)
        {
            _daFactory = daFactory;

            _fireBase = new FireBase(configurations.FirebaseConnection);

            _imagga = new ImaggaApi(configurations.ImaggaKey.ImaggaApiKey, configurations.ImaggaKey.ImaggaApiSecret);

            _nasaApi = new NasaApi(configurations.CurrentNasaApiKey);

            _wordAssociations = new(configurations.WordAssociationsApiKey);
        }

        public async Task<IEnumerable<MediaGroupe>> SearchMedia(string keyWord)
        {
            keyWord = keyWord
                .ToLower()
                .Trim();

            using var unitOfWork = _daFactory.GetDataAccess();

            var medias = await unitOfWork
                                                .MediaSearchRepository
                                                .Search(keyWord);
            
            if (medias.Any())
            {
                return medias;
            }

            return await GetNewFromNasa(keyWord);
        }

        public async Task<IEnumerable<MediaGroupe>> SearchMedia(string keyWord, int skip)
        {
            keyWord = keyWord.ToLower();
            return await GetNewFromNasa(keyWord, skip);
        }

        public async Task<IEnumerable<string>> GetSearchWords()
        {
            using var unitOfWork = _daFactory.GetDataAccess();

            var result = await unitOfWork
                .SearchWordRepository
                .GetAll();

            return result.Select(s => s.SearchWord)
                .Distinct()
                .Take(10);
        }

        private async Task<IEnumerable<MediaGroupe>> GetNewFromNasa(string keyWord, int skip = 0)
        {
            using var unitOfWork = _daFactory.GetDataAccess();

            var mediasFromNasa = await _nasaApi.SearchMedia(keyWord, skip);

            await unitOfWork.MediaSearchRepository
                .InsertMany(mediasFromNasa);
            await unitOfWork.Complete();

            return await unitOfWork
                            .MediaSearchRepository
                            .Search(keyWord);
        }



        public async Task<IEnumerable<ImaggaTag>> GetMediaTags(MediaGroupe media)
        {
            IEnumerable<ImaggaTag> tags = await GetTagsFromDB(media);

            if (tags.Any())
            {
                return tags;
            }

            tags = await TagImage(media.PreviewUrl);

            using var unitOfWork = _daFactory.GetDataAccess();

            if (tags != null)
            {
                await unitOfWork
                    .MediaSearchRepository
                    .AddTags(media, tags);
                await unitOfWork.Complete();
                return await GetTagsFromDB(media);
            }

            throw new Exception("Can not parse image");
        }

        private async Task<IEnumerable<ImaggaTag>> GetTagsFromDB(MediaGroupe media)
        {
            using var unitOfWork = _daFactory.GetDataAccess();
            return await unitOfWork
                .ImaggaTagRepository
                .FindAll(t => t.MediaGroupeId == media.Id);
        }

        private async Task<List<ImaggaTag>> TagImage(string imageUrl)
        {
            var imaggTag = await _imagga.AutoTagging(imageUrl);
            return imaggTag.GetTags();
        }
    }
}
