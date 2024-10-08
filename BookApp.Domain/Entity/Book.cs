﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookApp.Domain.Entity
{
    public class Book
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Author { get; set; }
        public int Year { get; set; }
    }
}