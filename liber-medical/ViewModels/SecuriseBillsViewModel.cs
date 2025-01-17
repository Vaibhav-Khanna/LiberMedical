﻿using libermedical.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using System.Reflection;
using libermedical.Managers;
using libermedical.Models;
using libermedical.Services;
using libermedical.Helpers;

namespace libermedical.ViewModels
{
    class SecuriseBillsViewModel : ViewModelBase
    {
        string fileLink;

        private Stream m_pdfDocumentStream;
        public Stream PdfDocumentStream
        {
            get
            {
                return m_pdfDocumentStream;
            }
            set
            {
                m_pdfDocumentStream = value;
                RaisePropertyChanged();
            }
        }

        private string _title;
        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                RaisePropertyChanged();
            }

        }

        public SecuriseBillsViewModel()
        {
        }

        bool isTeledeclaration;

        public override void Init(object initData)
        {
            base.Init(initData);

            if (initData != null)
            {
                if (initData is Teledeclaration)
                {
                    isTeledeclaration = true;
                    var teledeclaration = initData as Teledeclaration;
                    Title = teledeclaration.Label;
                    if (teledeclaration.FilePath.StartsWith("/"))
                        DownloadFile(teledeclaration.FilePath.Remove(0, 1));
                    else
                        DownloadFile(teledeclaration.FilePath);
                }
                else if (initData is Ordonnance)
                {
                    var ordonnance = initData as Ordonnance;
                    Title = ordonnance.PatientName;
                    if (ordonnance.Attachments.Count > 0)
                        if (ordonnance.Attachments[0].StartsWith("/"))
                            DownloadFile(ordonnance.Attachments[0].Remove(0, 1));
                        else
                            DownloadFile(ordonnance.Attachments[0]);
                }
                else if (initData is Document)
                {
                    var document = initData as Document;
                    Title = document.Patient?.Fullname;
                    if (document.AttachmentPath.StartsWith("/"))
                        DownloadFile(document.AttachmentPath.Remove(0, 1));
                    else
                        DownloadFile(document.AttachmentPath);
                }
                else if (initData is Invoice)
                {
                    var invoice = initData as Invoice;
                    Title = invoice.Label;
                   
                    if (invoice.IsBill)
                        DownloadBill(invoice.FilePath);
                    else
                    DownloadFile(invoice.FilePath);
                }
            }
        }


        protected override void ViewIsAppearing(object sender, EventArgs e)
        {
            base.ViewIsAppearing(sender, e);


        }

      
        public Command ShareCommand => new Command(async () =>
        {
            if (string.IsNullOrEmpty(fileLink))
                return;

            if (isTeledeclaration)
            {
                await CoreMethods.PopPageModel(fileLink, modal: true);
                return;
            }


            var isAllow = await App.AskForStoragePermission();

            if (!isAllow)
                return;

            Acr.UserDialogs.UserDialogs.Instance.ShowLoading("");

            await DependencyService.Get<IShare>().ShareRemoteFile(fileLink, "PDF_" + DateTime.Today.Ticks + ".pdf");

            Acr.UserDialogs.UserDialogs.Instance.HideLoading();

        });

        private async void DownloadFile(string filePath)
        {
            await Task.Delay(800);
            fileLink = string.Format(Constants.RestUrl + "file?path=" + filePath + "&" + string.Concat("token=", Settings.Token));
            PdfDocumentStream = await new RestService<BaseDTO>("file").DownloadFile(filePath, false);
        }

        private async void DownloadBill(string filePath)
        {
            await Task.Delay(800);
            fileLink = filePath;
            PdfDocumentStream = await new RestService<BaseDTO>("file").DownloadBill(filePath);
        }


        public ICommand BackCommand => new Command(async () => await Application.Current.MainPage.Navigation.PopModalAsync());
    }
}
 
