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
    public partial class tblincedentconfig: IObjectWithChangeTracker, INotifyPropertyChanged
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
        public Nullable<short> ModuleId
        {
            get { return _moduleId; }
            set
            {
                if (_moduleId != value)
                {
                    _moduleId = value;
                    OnPropertyChanged("ModuleId");
                }
            }
        }
        private Nullable<short> _moduleId;
    
        [DataMember]
        public string Actor
        {
            get { return _actor; }
            set
            {
                if (_actor != value)
                {
                    _actor = value;
                    OnPropertyChanged("Actor");
                }
            }
        }
        private string _actor;
    
        [DataMember]
        public string Description
        {
            get { return _description; }
            set
            {
                if (_description != value)
                {
                    _description = value;
                    OnPropertyChanged("Description");
                }
            }
        }
        private string _description;
    
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
        public string CalculationType
        {
            get { return _calculationType; }
            set
            {
                if (_calculationType != value)
                {
                    _calculationType = value;
                    OnPropertyChanged("CalculationType");
                }
            }
        }
        private string _calculationType;
    
        [DataMember]
        public string FixedValue
        {
            get { return _fixedValue; }
            set
            {
                if (_fixedValue != value)
                {
                    _fixedValue = value;
                    OnPropertyChanged("FixedValue");
                }
            }
        }
        private string _fixedValue;
    
        [DataMember]
        public Nullable<int> LowerLimit
        {
            get { return _lowerLimit; }
            set
            {
                if (_lowerLimit != value)
                {
                    _lowerLimit = value;
                    OnPropertyChanged("LowerLimit");
                }
            }
        }
        private Nullable<int> _lowerLimit;
    
        [DataMember]
        public Nullable<int> UpperLimit
        {
            get { return _upperLimit; }
            set
            {
                if (_upperLimit != value)
                {
                    _upperLimit = value;
                    OnPropertyChanged("UpperLimit");
                }
            }
        }
        private Nullable<int> _upperLimit;
    
        [DataMember]
        public Nullable<int> TitleId
        {
            get { return _titleId; }
            set
            {
                if (_titleId != value)
                {
                    _titleId = value;
                    OnPropertyChanged("TitleId");
                }
            }
        }
        private Nullable<int> _titleId;
    
        [DataMember]
        public string IncedentDesc
        {
            get { return _incedentDesc; }
            set
            {
                if (_incedentDesc != value)
                {
                    _incedentDesc = value;
                    OnPropertyChanged("IncedentDesc");
                }
            }
        }
        private string _incedentDesc;
    
        [DataMember]
        public string Point_Value
        {
            get { return _point_Value; }
            set
            {
                if (_point_Value != value)
                {
                    _point_Value = value;
                    OnPropertyChanged("Point_Value");
                }
            }
        }
        private string _point_Value;
    
        [DataMember]
        public string IsEnable
        {
            get { return _isEnable; }
            set
            {
                if (_isEnable != value)
                {
                    _isEnable = value;
                    OnPropertyChanged("IsEnable");
                }
            }
        }
        private string _isEnable;
    
        [DataMember]
        public Nullable<sbyte> ActionId
        {
            get { return _actionId; }
            set
            {
                if (_actionId != value)
                {
                    _actionId = value;
                    OnPropertyChanged("ActionId");
                }
            }
        }
        private Nullable<sbyte> _actionId;

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
