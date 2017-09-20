using System;
using FreshMvvm;
using libermedical.Models;
using libermedical.Services;

namespace libermedical.ViewModels
{
	public class TeledeclarationSecureActionViewModel:FreshBasePageModel
	{
		private readonly IStorageService<Teledeclaration> _teledeclarationService;

		private Teledeclaration _teledeclaration;

		public Teledeclaration Teledeclaration
		{
			get { return _teledeclaration;}
			set
			{
				_teledeclaration = value;
				if (_teledeclaration != null)
					CanValidate = _teledeclaration.Status == Enums.StatusEnum.waiting ? true : false;
				RaisePropertyChanged();
			}
		}
		public bool CanValidate { get; set; }

		public override void Init(object initData)
		{
			base.Init(initData);


		}

	}
}
