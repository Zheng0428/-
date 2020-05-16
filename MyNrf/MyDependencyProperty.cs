using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Forms;
namespace MyNrf
{

    public class MyDependencyProperty
    {

        private Type _propertyType;
        public Type PropertyType
        {
            get { return _propertyType; }
        }
        private Type _ownerType;
        public Type OwnerType
        {
            get { return _ownerType; }
        }
        private string _name;
        private object _defaultValue;
        private MyPropertyMetadata _propertyMetadata;
        public MyPropertyMetadata PropertyMetadata
        {
            get { return _propertyMetadata; }
        }
        public object DefaultValue
        {
            get
            {
                if (_propertyMetadata != null)
                {
                    return _propertyMetadata.DefaultValue;
                }
                return null;
            }
        }
        public string Name { get { return _name; } }
        //构造函数私有化保证只能由类的静态方法实例化.当然，如果为public也是可以的，这里做只是为了统一构造入口,
        //便于管理和维护。
        private MyDependencyProperty(string name, Type propertyType, Type ownerType, MyPropertyMetadata propertyMetadata)
        {
            _propertyType = propertyType;
            _ownerType = ownerType;
            _name = name;
            _propertyMetadata = propertyMetadata;

        }
        //依赖属性实例化工厂方法类.
        public static MyDependencyProperty Register(string name, Type propertyType, Type ownerType, MyPropertyMetadata propertyMetadata)
        {
            if (propertyMetadata != null && propertyMetadata.DefaultValue.GetType() != propertyType)
            {
                throw new Exception(string.Format("the type of defaultValue is not {0}", propertyType.Name));
            }
            MyDependencyProperty dp = new MyDependencyProperty(name, propertyType, ownerType, propertyMetadata);
            return dp;
        }
        public static MyDependencyProperty Register(string name, Type propertyType, Type ownerType)
        {

            MyDependencyProperty dp = new MyDependencyProperty(name, propertyType, ownerType, new MyPropertyMetadata());
            return dp;
        }

    }

    public delegate void MyPropertyChangedCallback(MyDependencyObject d, MyDependencyPropertyChangedEventArgs e);
    /// <summary>
    /// 属性元数据类，目标是为了存储属性的缺省值和值发生变化时的回调函数
    /// 其实在注册依赖属性的时候，可以直接将该类的成员加上，但这样的做法不利于扩展，
    /// 因为你可以继承这个MyPropertyMetadata这个类，增加信息量，而不需要更改依赖
    /// 属性的结构。
    /// </summary>
    public class MyPropertyMetadata
    {
        public MyPropertyChangedCallback PropertyChangedCallback { get; private set; }
        public object DefaultValue { get; private set; }
        public MyPropertyMetadata(object DefaultValue =null, MyPropertyChangedCallback Callback = null)
        {
            this.PropertyChangedCallback = Callback;
            this.DefaultValue = DefaultValue;
        }

    }
    /// <summary>
    /// 属性发生改变时的参数
    /// </summary>
    public class MyDependencyPropertyChangedEventArgs
    {

        // 摘要:
        //     获取发生更改之后的属性的值。
        //
        // 返回结果:
        //     发生更改之后的属性值。
        public object NewValue { get; private set; }
        //
        // 摘要:
        //     获取发生更改之前的属性的值。
        //
        // 返回结果:
        //     发生更改之前的属性值。
        public object OldValue { get; private set; }
        //
        // 摘要:
        //     获取发生值更改的依赖项属性的标识符。
        //
        // 返回结果:
        //     发生值更改的依赖项属性的标识符字段。
        public MyDependencyProperty Property { get; private set; }
        public MyDependencyPropertyChangedEventArgs(object NewValue, object OldValue, MyDependencyProperty Property)
        {
            this.NewValue = NewValue;
            this.OldValue = OldValue;
            this.Property = Property;
        }
    }
    /// <summary>
    /// 绑定设置类。
    /// </summary>
    public class MyBinding
    {
          public MyBinding() 
        {
            ;
        }
        public MyBinding(object TargetObject, string PropertyName) 
        {
            this.TargetObject = TargetObject;
            this.PropertyName = PropertyName;
        }
        public object TargetObject { get; set; }
        public string PropertyName { get; set; }
    }
    /// <summary>
    /// 依赖对像，主要提供属性值和属性绑定的管理。
    /// </summary>
    public  class MyDependencyObject 
    {
        private IDictionary<MyDependencyProperty, object> _dict = new Dictionary<MyDependencyProperty, object>();
        private IDictionary<MyDependencyProperty, MyBinding> _bindings = new Dictionary<MyDependencyProperty, MyBinding>();
        public event EventHandler DependencyPropertyChanged;

        private  void OnEvent()
        {
            if (this.DependencyPropertyChanged != null)
            {
                this.DependencyPropertyChanged(this, new EventArgs());
            }
        }
        public  void SetValue(MyDependencyProperty p, object val) 
        {

            //如果设置值是默认值，则可不保存，以节省空间.
            object theOldValue = null;
            if (_dict.ContainsKey(p))
            {
                theOldValue = _dict[p];
                //如果已有设置值，且等于当前设置值，则退出
                if (theOldValue == val)
                {
                    return;
                }
                //如果设置值等于默认值，则删除已有的设置值。
                if (p.DefaultValue == val)
                {
                    _dict.Remove(p);
                    return;
                }
                //设置新的字典值
                _dict[p] = val;
            }
            else
            {
                //如果设置值不等于默认值，则增加设置值,否则不做任何设置。
                if (p.DefaultValue != val)
                {
                    _dict.Add(p, val);
                }
                else
                {
                    return;
                }

            }

            if (p.PropertyMetadata != null && p.PropertyMetadata.PropertyChangedCallback != null)
            {
                MyDependencyPropertyChangedEventArgs theArgs =
                    new MyDependencyPropertyChangedEventArgs(val, theOldValue, p);
                p.PropertyMetadata.PropertyChangedCallback(this, theArgs);
            }
            //如果是双向绑定，则需要同步数据到绑定数据源，这里假设需要双向绑定.
            if (_bindings.ContainsKey(p) == true)
            {
                MyBinding theBinding = _bindings[p];
                if (theBinding.TargetObject != null && theBinding.PropertyName != "")
                {
                    System.Reflection.PropertyInfo thePI = theBinding.TargetObject.GetType().GetProperty(theBinding.PropertyName);
                    if (thePI != null && thePI.CanWrite == true)
                    {
                        //对于有索引的设置值比较复杂一点，可利用反射来进行，这里只是演示简单属性。
                        //注意，如果目标类实现了INotifyPropertyChanged接口，并有修改触发机制，那么这里的设置
                        //会触发目标属性改变事件，就会触发MyDependencyObject_PropertyChanged执行，
                        //而MyDependencyObject_PropertyChanged里又调用了SetValue函数，这就会死循环，这也是
                        //为什么前面的代码中为什么要判断如果已经有的设置值等于当前设置新值直接退出的缘故，就是
                        //为了阻止死循环.当然，在目标属性中set里面做判断也可以，但这里一定要做，
                        //原因大家可以自己想一下。
                        thePI.SetValue(theBinding.TargetObject, val, null);
                        OnEvent();  
                   
                    }
                }
            }
        }
        public object GetValue(MyDependencyProperty p)
        {
            //如果被动画控制，返回动画计算值。（可能会用到p.Name）
            //如果有本地值，返回本地值
            if (_dict.ContainsKey(p))
            {
                return _dict[p];
            }
            //如果有Style，则返回Style的值
            //返回从可视化树中继承的值
            //最后, 返回依赖属性的DefaultValue
            return p.DefaultValue;
        }
        /// <summary>
        /// 设置绑定属性，一样是模拟微软的干活，只不过微软的这个方法不是在依赖对象里实现的,
        /// 而是在UIElement里实现的.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="Binding"></param>
        public void SetBinding(MyDependencyProperty p, MyBinding Binding)
        {
            MyBinding theOld = null;
            //需要先将老的绑定找到并记录，因为需要解除挂接.
            if (_bindings.ContainsKey(p))
            {
                theOld = _bindings[p];
                _bindings[p] = Binding;
            }
            else
            {
                _bindings.Add(p, Binding);
            }
            //删除旧的绑定.
            if (theOld != null)
            {
                if (theOld.TargetObject is INotifyPropertyChanged)
                {
                    ((INotifyPropertyChanged)theOld.TargetObject).PropertyChanged -= new PropertyChangedEventHandler(MyDependencyObject_PropertyChanged);
                }
            }
            //如果是单向绑定或者双向绑定则需要以下挂接。如果只是Onetime则不必要.
            if (Binding.TargetObject is INotifyPropertyChanged)
            {
                ((INotifyPropertyChanged)Binding.TargetObject).PropertyChanged += new PropertyChangedEventHandler(MyDependencyObject_PropertyChanged);
            }
        }
        /// <summary>
        /// 目标属性发生变化时的处理事件方法。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
         private  void MyDependencyObject_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            MyDependencyProperty p = null;
            //找到绑定属性所在的依赖属性
            foreach (var b in _bindings)
            {
                if (b.Value.PropertyName == e.PropertyName)
                {
                    p = b.Key;
                    break;
                }
            }
            //不为空则处理.
            if (p != null)
            {
                System.Reflection.PropertyInfo thePI = sender.GetType().GetProperty(e.PropertyName);
                if (thePI != null && thePI.CanRead == true)
                {
                    object theVal = thePI.GetValue(sender, null);
                    SetValue(p, theVal);
                    //如果目标类INotifyPropertyChanged，绑定模式是ontime,则下面的代码就是要接触与目标属性的挂接.
                    if (sender is INotifyPropertyChanged)
                    {
                 
                        ((INotifyPropertyChanged)sender).PropertyChanged += new PropertyChangedEventHandler(MyDependencyObject_PropertyChanged);
                    }
                }
            }
        }
    }

    public class MyDendencyControl : MyDependencyObject
    {
        public static readonly MyDependencyProperty ContentDependencyProperty =
            MyDependencyProperty.Register("Content", typeof(object), typeof(MyDendencyControl));
        //封装成普通属性的依赖属性，注意调用的是基类的相关方法。
        
        public MyDendencyControl()
        {
            ;
        }
        public MyDendencyControl(object Content)
        {
            this.Content = Content;
        }
        public object Content
        {
            get
            {
                return GetValue(ContentDependencyProperty);
            }
            set
            {
                SetValue(ContentDependencyProperty, value);
            }
        }
        public override string ToString()
        {
            return Content.ToString();
        }

    }

    public class MyNotifyPropertyClass : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string PropertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
            }

        }
        private object _Value;
        public object Value
        {
            get
            {
                return _Value;
            }
            set
            {
                if (_Value != value) //这是比较好的习惯，可以提供性能.
                {
                    _Value = value;
                    RaisePropertyChanged("Value");
                }
            }
        }
        public    MyNotifyPropertyClass()
        {
            ;        
        }
       public MyNotifyPropertyClass(object value)
        {
            _Value = value;
        }
        /// <summary>
        /// 设定初始值，不触发属性改变
        /// </summary>
        /// <param name="value"></param>
        public void SetValue(object value)
        {
         _Value = value;
        }
        public override string ToString()
        {
            return Value.ToString();
        }
    }


}
