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

namespace DomainModel.Services
{
    public class MediaService : IMediaService
    {
        readonly IUnitOfWork _unitOfWork;

        readonly ImaggaApi _imagga;

        public MediaService(IDbFactory dbFactory, MyConfigurations configurations)
        {
            _unitOfWork = dbFactory.GetDataAccess();
            _imagga = new ImaggaApi(configurations.ImaggaApiKey, configurations.ImaggaApiSecret);
        }


        public async Task<List<string>> SearchMedia(string keyWord)
        {
            var searchWord = keyWord.ToLower();
            var imgsTemp = await _unitOfWork.MediaSearchRepository.Search(searchWord);
            
            var imags = RootToString(imgsTemp);

            var result = new List<string>();

            List<Task<Tuple<string, List<ImaggaTag>>>> tasks = new();
            foreach(var im in imags)
            {
                tasks.Add(TagImage(im));
            }
            
            var imageAndtags = await Task.WhenAll(tasks);
            
            var reletedWords = GetReletedWords(
                imageAndtags.Select(t => t.Item2), searchWord);
            
            result.AddRange(from t in imageAndtags
                            where IsImageReletedToSearchWord(reletedWords, t.Item2)
                            select t.Item1);

            // no data from db or firebase => call nasa => call imagga => save relevt data to db
            return result;
        }

        private async Task<Tuple<string, List<ImaggaTag>>> TagImage(string imageUrl)
        {

            var imaggTag = await _imagga.AutoTagging(imageUrl);
            var tags = imaggTag.GetTags();

            return new(imageUrl, tags);
        }

        private List<string> GetReletedWords(IEnumerable<List<ImaggaTag>> tags, string searchWord)
        {
            List<string> result = new();
            foreach(var tag in tags)
            {
                if (tag != null && tag.Where(t => t.Tag.Contains(searchWord)).Any())
                {
                    result.AddRange(tag.Select(t => t.Tag));
                }
            }
            return result;
        }

        private bool IsImageReletedToSearchWord(List<string> reletedWords, List<ImaggaTag> tags)
        {
            var tagsWords = tags.Select(t => t.Tag);
            if(tagsWords.Intersect(reletedWords).Any())
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
