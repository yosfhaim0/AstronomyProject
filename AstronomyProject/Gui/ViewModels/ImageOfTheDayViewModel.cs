using ApiRequests.Nasa;
using DataAccess.UnitOfWork;
using Models;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools;

namespace Gui.ViewModels
{
    public class ImageOfTheDayViewModel : BindableBase
    {
        IUnitOfWork _unitOfWork;

        public ImageOfTheDayViewModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public ObservableCollection<ImageOfTheDay> Images { get; set; } = new();
        DelegateCommand _load;
        
        public DelegateCommand Load => _load ??=
            new DelegateCommand(async () =>
            {
                var img = await _unitOfWork.ImageOfTheDayRepository.GetImageOfTheDayFromNasa();
                await _unitOfWork.Complete();
                Images.Clear();
                Images.Add(img);
            });
    }
}
