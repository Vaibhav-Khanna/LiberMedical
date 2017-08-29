﻿using System;
using libermedical.Enums;

namespace libermedical.Models
{
    public class Ordonnance : BaseDTO
    {

        public long Reference { set; get; }
        public DateTime AddDate { set; get; }
        public Patient Patient { set; get; }
        public StatusEnum Status { set; get; }

        public string StatusString => Status == StatusEnum.Attente
            ? "En attente"
            : Status == StatusEnum.Traite
                ? "Traité"
                : "Refusé";
    }
}
