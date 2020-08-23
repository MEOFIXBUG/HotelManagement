using System.Data.Entity.Infrastructure.Annotations;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using Login2.Properties;
using System.Runtime.CompilerServices;
using System.ComponentModel.DataAnnotations;
using System.Linq;

/// <summary>
/// Abstract base class for all models.
/// </summary>
public abstract class BaseModel : INotifyPropertyChanged, IDataErrorInfo
{
    #region constants

    private static List<PropertyInfo> _propertyInfos;

    #endregion

    #region events

    /// <summary>
    /// Occurs when a property value changes.
    /// </summary>
    public event PropertyChangedEventHandler PropertyChanged;

    #endregion

    #region constructors and destructors

    /// <summary>
    /// Default constructor.
    /// </summary>
    public BaseModel()
    {
        InitCommands();
    }

    #endregion

    #region explicit interfaces

    /// <summary>
    /// Gets an error message indicating what is wrong with this object.
    /// </summary>
    /// <returns>
    /// An error message indicating what is wrong with this object. The default is an empty string ("").
    /// </returns>
    public string Error => string.Empty;

    /// <summary>
    /// Gets the error message for the property with the given name.
    /// </summary>
    /// <returns>
    /// The error message for the property. The default is an empty string ("").
    /// </returns>
    /// <param name="columnName">The name of the property whose error message to get. </param>
    public string this[string columnName]
    {
        get
        {
            CollectErrors();
            return Errors.ContainsKey(columnName) ? Errors[columnName] : string.Empty;
        }
    }

    #endregion

    #region methods

    /// <summary>
    /// Override this method in derived types to initialize command logic.
    /// </summary>
    protected virtual void InitCommands()
    {
    }

    /// <summary>
    /// Can be overridden by derived types to react on the finisihing of error-collections.
    /// </summary>
    protected virtual void OnErrorsCollected()
    {
    }

    /// <summary>
    /// Raises the <see cref="PropertyChanged" /> event.
    /// </summary>
    /// <param name="propertyName">The name of the property which value has changed.</param>
    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    /// <summary>
    /// Is called by the indexer to collect all errors and not only the one for a special field.
    /// </summary>
    /// <remarks>
    /// Because <see cref="HasErrors" /> depends on the <see cref="Errors" /> dictionary this
    /// ensures that controls like buttons can switch their state accordingly.
    /// </remarks>
    private void CollectErrors()
    {
        Errors.Clear();
        PropertyInfos.ForEach(
            prop =>
            {
                        //Validate generically
                        var errors = new List<string>();
                var isValid = TryValidateProperty(prop, errors);
                if (!isValid)

                    Errors.Add(prop.Name, errors.First());
            });
        // we have to this because the Dictionary does not implement INotifyPropertyChanged            
        OnPropertyChanged(nameof(HasErrors));
        OnPropertyChanged(nameof(IsOk));
        // commands do not recognize property changes automatically
        OnErrorsCollected();
    }

    #endregion

    #region properties

    /// <summary>
    /// Indicates whether this instance has any errors.
    /// </summary>
    public bool HasErrors => Errors.Any();

    /// <summary>
    /// The opposite of <see cref="HasErrors" />.
    /// </summary>
    /// <remarks>
    /// Exists for convenient binding only.
    /// </remarks>
    public bool IsOk => !HasErrors;

    /// <summary>
    /// Retrieves a list of all properties with attributes required for <see cref="IDataErrorInfo" /> automation.
    /// </summary>
    protected List<PropertyInfo> PropertyInfos
    {
        get
        {
            return _propertyInfos
                   ?? (_propertyInfos =
                       GetType()
                           .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                           .Where(prop => prop.IsDefined(typeof(RequiredAttribute), true) || prop.IsDefined(typeof(MaxLengthAttribute), true))
                           .ToList());
        }
    }

    /// <summary>
    /// A dictionary of current errors with the name of the error-field as the key and the error
    /// text as the value.
    /// </summary>
    private Dictionary<string, string> Errors { get; } = new Dictionary<string, string>();

    #endregion
    private bool TryValidateProperty(PropertyInfo propertyInfo, List<string> propertyErrors)
    {
        var results = new List<ValidationResult>();
        var context = new ValidationContext(this) { MemberName = propertyInfo.Name };

        var propertyValue = propertyInfo?.GetValue(this);

        // Validate the property
        var isValid = Validator.TryValidateProperty(propertyValue, context, results);

        if (results.Any())
        {
            propertyErrors.AddRange(results.Select(c => c.ErrorMessage));
        }
        return isValid;


    }
}