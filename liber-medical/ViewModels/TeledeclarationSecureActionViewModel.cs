using System;
using FreshMvvm;
using libermedical.Models;
using libermedical.Services;

namespace libermedical.ViewModels
{
	public class TeledeclarationSecureActionViewModel:FreshBasePageModel
	{
		private readonly IStorageService<Teledeclaration> _teledeclarationService;

		public Teledeclaration Teledeclaration { get; set; }

		public override void Init(object initData)
		{
			base.Init(initData);
			if (initData != null)
			{
				//Teledeclaration = initData as Teledeclaration;

			}
		}

	}
}
