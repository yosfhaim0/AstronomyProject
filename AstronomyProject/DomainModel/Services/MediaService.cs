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
            _unitOfWork = dbFactory.GetDataAccess(); ;
            
            _imagga = new ImaggaApi(configurations.ImaggaKey.ImaggaApiKey, configurations.ImaggaKey.ImaggaApiSecret);
            
            _wordAssociations = new(configurations.WordAssociationsApiKey);
        }


        public async Task<List<string>> SearchMedia(string keyWord)
        {
            var searchWord = keyWord.ToLower();
            var imags = await _unitOfWork.MediaSearchRepository.Search(searchWord);
            //new List<string> { "https://upload.wikimedia.org/wikipedia/commons/thumb/e/e1/FullMoon2010.jpg/440px-FullMoon2010.jpg" };//
            //return imags.ToList();

            var result = new List<string>();

            List<Task<Tuple<string, List<ImaggaTag>>>> tasks = new();
            foreach (var im in imags)
            {
                tasks.Add(TagImage(im));
            }

            var imageAndTags = await Task.WhenAll(tasks);

            var reletedWords = await GetWordAssociations(searchWord);

            var res = (from t in imageAndTags
                       where IsImageReletedToSearchWord(reletedWords, t.Item2)
                       select t.Item1).ToList();

            result.AddRange(res);

            // no data from db or firebase => call nasa => call imagga => save relevt data to db
            return result;
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
            if (inter.Count() >= 1)
            {
                return  true;
            }

            return false;
        }

        
    }
}
