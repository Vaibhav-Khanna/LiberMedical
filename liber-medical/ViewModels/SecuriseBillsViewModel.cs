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

namespace libermedical.ViewModels
{
    class SecuriseBillsViewModel : ViewModelBase
    {
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

        public override void Init(object initData)
        {
            base.Init(initData);
            if (initData != null)
            {
                if (initData is Teledeclaration)
                {
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

            }
        }
        private async void DownloadFile(string filePath)
        {
            PdfDocumentStream = await new RestService<BaseDTO>("file").DownloadFile(filePath, false);
        }

        public ICommand BackCommand => new Command(async () => await Application.Current.MainPage.Navigation.PopModalAsync());
    }
}
