﻿using System;

namespace Gymme.ViewModel.Statistics
{
    public class TimeStatPoint : Base.ViewModel
    {
        public TimeStatPoint(DateTime start, DateTime fin)
            : this(start, fin - start)
        {
        }

        public TimeStatPoint(DateTime start, TimeSpan span)
        {
            Date = start;
            Span = span.TotalMinutes;
        }

        public double Span { get; private set; }

        public DateTime Date { get; private set; }
    }
}