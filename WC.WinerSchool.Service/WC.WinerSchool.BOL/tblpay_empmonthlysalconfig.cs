//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.Serialization;

namespace WC.WinerSchool.BOL
{
    [DataContract(IsReference = true)]
    public partial class tblpay_empmonthlysalconfig: IObjectWithChangeTracker, INotifyPropertyChanged
    {
        #region Primitive Properties
    
        [DataMember]
        public int Id
        {
            get { return _id; }
            set
            {
                if (_id != value)
                {
                    if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
                    {
                        throw new InvalidOperationException("The property 'Id' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
                    }
                    _id = value;
                    OnPropertyChanged("Id");
                }
            }
        }
        private int _id;
    
        [DataMember]
        public Nullable<int> Year
        {
            get { return _year; }
            set
            {
                if (_year != value)
                {
                    _year = value;
                    OnPropertyChanged("Year");
                }
            }
        }
        private Nullable<int> _year;
    
        [DataMember]
        public Nullable<sbyte> MonthId
        {
            get { return _monthId; }
            set
            {
                if (_monthId != value)
                {
                    _monthId = value;
                    OnPropertyChanged("MonthId");
                }
            }
        }
        private Nullable<sbyte> _monthId;
    
        [DataMember]
        public string EmpId
        {
            get { return _empId; }
            set
            {
                if (_empId != value)
                {
                    _empId = value;
                    OnPropertyChanged("EmpId");
                }
            }
        }
        private string _empId;
    
        [DataMember]
        public Nullable<int> CatId
        {
            get { return _catId; }
            set
            {
                if (_catId != value)
                {
                    _catId = value;
                    OnPropertyChanged("CatId");
                }
            }
        }
        private Nullable<int> _catId;
    
        [DataMember]
        public Nullable<double> BasicPay
        {
            get { return _basicPay; }
            set
            {
                if (_basicPay != value)
                {
                    _basicPay = value;
                    OnPropertyChanged("BasicPay");
                }
            }
        }
        private Nullable<double> _basicPay;
    
        [DataMember]
        public Nullable<double> TotalGross
        {
            get { return _totalGross; }
            set
            {
                if (_totalGross != value)
                {
                    _totalGross = value;
                    OnPropertyChanged("TotalGross");
                }
            }
        }
        private Nullable<double> _totalGross;
    
        [DataMember]
        public Nullable<double> TotalDeduction
        {
            get { return _totalDeduction; }
            set
            {
                if (_totalDeduction != value)
                {
                    _totalDeduction = value;
                    OnPropertyChanged("TotalDeduction");
                }
            }
        }
        private Nullable<double> _totalDeduction;
    
        [DataMember]
        public Nullable<double> NetPay
        {
            get { return _netPay; }
            set
            {
                if (_netPay != value)
                {
                    _netPay = value;
                    OnPropertyChanged("NetPay");
                }
            }
        }
        private Nullable<double> _netPay;
    
        [DataMember]
        public Nullable<int> TotalWorking
        {
            get { return _totalWorking; }
            set
            {
                if (_totalWorking != value)
                {
                    _totalWorking = value;
                    OnPropertyChanged("TotalWorking");
                }
            }
        }
        private Nullable<int> _totalWorking;
    
        [DataMember]
        public Nullable<int> TotalWorked
        {
            get { return _totalWorked; }
            set
            {
                if (_totalWorked != value)
                {
                    _totalWorked = value;
                    OnPropertyChanged("TotalWorked");
                }
            }
        }
        private Nullable<int> _totalWorked;
    
        [DataMember]
        public string Comment
        {
            get { return _comment; }
            set
            {
                if (_comment != value)
                {
                    _comment = value;
                    OnPropertyChanged("Comment");
                }
            }
        }
        private string _comment;
    
        [DataMember]
        public Nullable<sbyte> Approval
        {
            get { return _approval; }
            set
            {
                if (_approval != value)
                {
                    _approval = value;
                    OnPropertyChanged("Approval");
                }
            }
        }
        private Nullable<sbyte> _approval;
    
        [DataMember]
        public Nullable<sbyte> Reject
        {
            get { return _reject; }
            set
            {
                if (_reject != value)
                {
                    _reject = value;
                    OnPropertyChanged("Reject");
                }
            }
        }
        private Nullable<sbyte> _reject;
    
        [DataMember]
        public Nullable<sbyte> Payed
        {
            get { return _payed; }
            set
            {
                if (_payed != value)
                {
                    _payed = value;
                    OnPropertyChanged("Payed");
                }
            }
        }
        private Nullable<sbyte> _payed;
    
        [DataMember]
        public Nullable<sbyte> Configured
        {
            get { return _configured; }
            set
            {
                if (_configured != value)
                {
                    _configured = value;
                    OnPropertyChanged("Configured");
                }
            }
        }
        private Nullable<sbyte> _configured;
    
        [DataMember]
        public Nullable<double> AdvSal
        {
            get { return _advSal; }
            set
            {
                if (_advSal != value)
                {
                    _advSal = value;
                    OnPropertyChanged("AdvSal");
                }
            }
        }
        private Nullable<double> _advSal;
    
        [DataMember]
        public Nullable<double> Advanceamount
        {
            get { return _advanceamount; }
            set
            {
                if (_advanceamount != value)
                {
                    _advanceamount = value;
                    OnPropertyChanged("Advanceamount");
                }
            }
        }
        private Nullable<double> _advanceamount;
    
        [DataMember]
        public Nullable<double> Deductionpercent
        {
            get { return _deductionpercent; }
            set
            {
                if (_deductionpercent != value)
                {
                    _deductionpercent = value;
                    OnPropertyChanged("Deductionpercent");
                }
            }
        }
        private Nullable<double> _deductionpercent;
    
        [DataMember]
        public Nullable<int> Status
        {
            get { return _status; }
            set
            {
                if (_status != value)
                {
                    _status = value;
                    OnPropertyChanged("Status");
                }
            }
        }
        private Nullable<int> _status;

        #endregion
        #region ChangeTracking
    
        protected virtual void OnPropertyChanged(String propertyName)
        {
            if (ChangeTracker.State != ObjectState.Added && ChangeTracker.State != ObjectState.Deleted)
            {
                ChangeTracker.State = ObjectState.Modified;
            }
            if (_propertyChanged != null)
            {
                _propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    
        protected virtual void OnNavigationPropertyChanged(String propertyName)
        {
            if (_propertyChanged != null)
            {
                _propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    
        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged{ add { _propertyChanged += value; } remove { _propertyChanged -= value; } }
        private event PropertyChangedEventHandler _propertyChanged;
        private ObjectChangeTracker _changeTracker;
    
        [DataMember]
        public ObjectChangeTracker ChangeTracker
        {
            get
            {
                if (_changeTracker == null)
                {
                    _changeTracker = new ObjectChangeTracker();
                    _changeTracker.ObjectStateChanging += HandleObjectStateChanging;
                }
                return _changeTracker;
            }
            set
            {
                if(_changeTracker != null)
                {
                    _changeTracker.ObjectStateChanging -= HandleObjectStateChanging;
                }
                _changeTracker = value;
                if(_changeTracker != null)
                {
                    _changeTracker.ObjectStateChanging += HandleObjectStateChanging;
                }
            }
        }
    
        private void HandleObjectStateChanging(object sender, ObjectStateChangingEventArgs e)
        {
            if (e.NewState == ObjectState.Deleted)
            {
                ClearNavigationProperties();
            }
        }
    
        protected bool IsDeserializing { get; private set; }
    
        [OnDeserializing]
        public void OnDeserializingMethod(StreamingContext context)
        {
            IsDeserializing = true;
        }
    
        [OnDeserialized]
        public void OnDeserializedMethod(StreamingContext context)
        {
            IsDeserializing = false;
            ChangeTracker.ChangeTrackingEnabled = true;
        }
    
        protected virtual void ClearNavigationProperties()
        {
        }

        #endregion
    }
}
