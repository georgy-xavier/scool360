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
    public partial class tblfine: IObjectWithChangeTracker, INotifyPropertyChanged
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
        public Nullable<double> Frequency
        {
            get { return _frequency; }
            set
            {
                if (_frequency != value)
                {
                    _frequency = value;
                    OnPropertyChanged("Frequency");
                }
            }
        }
        private Nullable<double> _frequency;
    
        [DataMember]
        public string Type
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
        private string _type;
    
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
        public Nullable<int> Finedate
        {
            get { return _finedate; }
            set
            {
                if (_finedate != value)
                {
                    _finedate = value;
                    OnPropertyChanged("Finedate");
                }
            }
        }
        private Nullable<int> _finedate;
    
        [DataMember]
        public Nullable<int> FineAmounttype
        {
            get { return _fineAmounttype; }
            set
            {
                if (_fineAmounttype != value)
                {
                    _fineAmounttype = value;
                    OnPropertyChanged("FineAmounttype");
                }
            }
        }
        private Nullable<int> _fineAmounttype;
    
        [DataMember]
        public Nullable<int> FineDuration
        {
            get { return _fineDuration; }
            set
            {
                if (_fineDuration != value)
                {
                    _fineDuration = value;
                    OnPropertyChanged("FineDuration");
                }
            }
        }
        private Nullable<int> _fineDuration;
    
        [DataMember]
        public Nullable<long> SyncDate
        {
            get { return _syncDate; }
            set
            {
                if (_syncDate != value)
                {
                    _syncDate = value;
                    OnPropertyChanged("SyncDate");
                }
            }
        }
        private Nullable<long> _syncDate;

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
