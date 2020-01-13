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
    public partial class tblfeeadvancetransaction: IObjectWithChangeTracker, INotifyPropertyChanged
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
        public Nullable<long> StudentId
        {
            get { return _studentId; }
            set
            {
                if (_studentId != value)
                {
                    _studentId = value;
                    OnPropertyChanged("StudentId");
                }
            }
        }
        private Nullable<long> _studentId;
    
        [DataMember]
        public string StudentName
        {
            get { return _studentName; }
            set
            {
                if (_studentName != value)
                {
                    _studentName = value;
                    OnPropertyChanged("StudentName");
                }
            }
        }
        private string _studentName;
    
        [DataMember]
        public string FeeName
        {
            get { return _feeName; }
            set
            {
                if (_feeName != value)
                {
                    _feeName = value;
                    OnPropertyChanged("FeeName");
                }
            }
        }
        private string _feeName;
    
        [DataMember]
        public string PeriodName
        {
            get { return _periodName; }
            set
            {
                if (_periodName != value)
                {
                    _periodName = value;
                    OnPropertyChanged("PeriodName");
                }
            }
        }
        private string _periodName;
    
        [DataMember]
        public Nullable<long> BatchId
        {
            get { return _batchId; }
            set
            {
                if (_batchId != value)
                {
                    _batchId = value;
                    OnPropertyChanged("BatchId");
                }
            }
        }
        private Nullable<long> _batchId;
    
        [DataMember]
        public Nullable<double> Amount
        {
            get { return _amount; }
            set
            {
                if (_amount != value)
                {
                    _amount = value;
                    OnPropertyChanged("Amount");
                }
            }
        }
        private Nullable<double> _amount;
    
        [DataMember]
        public Nullable<byte> Type
        {
            get { return _type; }
            set
            {
                if (_type != value)
                {
                    _type = value;
                    OnPropertyChanged("Type");
                }
            }
        }
        private Nullable<byte> _type;
    
        [DataMember]
        public string BillNo
        {
            get { return _billNo; }
            set
            {
                if (_billNo != value)
                {
                    _billNo = value;
                    OnPropertyChanged("BillNo");
                }
            }
        }
        private string _billNo;
    
        [DataMember]
        public Nullable<int> FeeId
        {
            get { return _feeId; }
            set
            {
                if (_feeId != value)
                {
                    _feeId = value;
                    OnPropertyChanged("FeeId");
                }
            }
        }
        private Nullable<int> _feeId;
    
        [DataMember]
        public Nullable<short> PeriodId
        {
            get { return _periodId; }
            set
            {
                if (_periodId != value)
                {
                    _periodId = value;
                    OnPropertyChanged("PeriodId");
                }
            }
        }
        private Nullable<short> _periodId;
    
        [DataMember]
        public string TempId
        {
            get { return _tempId; }
            set
            {
                if (_tempId != value)
                {
                    _tempId = value;
                    OnPropertyChanged("TempId");
                }
            }
        }
        private string _tempId;
    
        [DataMember]
        public string CreatedUser
        {
            get { return _createdUser; }
            set
            {
                if (_createdUser != value)
                {
                    _createdUser = value;
                    OnPropertyChanged("CreatedUser");
                }
            }
        }
        private string _createdUser;
    
        [DataMember]
        public Nullable<System.DateTime> CreatedDate
        {
            get { return _createdDate; }
            set
            {
                if (_createdDate != value)
                {
                    _createdDate = value;
                    OnPropertyChanged("CreatedDate");
                }
            }
        }
        private Nullable<System.DateTime> _createdDate;
    
        [DataMember]
        public Nullable<double> AdvanceBalance
        {
            get { return _advanceBalance; }
            set
            {
                if (_advanceBalance != value)
                {
                    _advanceBalance = value;
                    OnPropertyChanged("AdvanceBalance");
                }
            }
        }
        private Nullable<double> _advanceBalance;

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