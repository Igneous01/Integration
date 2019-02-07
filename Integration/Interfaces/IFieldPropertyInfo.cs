using Integration.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Integration.Interfaces
{
    public interface IFieldPropertyInfo
    {
        MemberTypes MemberType { get; }
        //
        // Summary:
        //     Gets the name of the current member.
        //
        // Returns:
        //     A System.String containing the name of this member.
        string Name { get; }
        //
        // Summary:
        //     Gets the class that declares this member.
        //
        // Returns:
        //     The Type object for the class that declares this member.
        Type DeclaringType { get; }
        //
        // Summary:
        //     Gets the class object that was used to obtain this instance of MemberInfo.
        //
        // Returns:
        //     The Type object through which this MemberInfo object was obtained.
        Type ReflectedType { get; }
        //
        // Summary:
        //     Gets a collection that contains this member's custom attributes.
        //
        // Returns:
        //     A collection that contains this member's custom attributes.
        IEnumerable<CustomAttributeData> CustomAttributes { get; }
        //
        // Summary:
        //     Gets a value that identifies a metadata element.
        //
        // Returns:
        //     A value which, in combination with System.Reflection.MemberInfo.Module, uniquely
        //     identifies a metadata element.
        //
        // Exceptions:
        //   T:System.InvalidOperationException:
        //     The current System.Reflection.MemberInfo represents an array method, such as
        //     Address, on an array type whose element type is a dynamic type that has not been
        //     completed. To get a metadata token in this case, pass the System.Reflection.MemberInfo
        //     object to the System.Reflection.Emit.ModuleBuilder.GetMethodToken(System.Reflection.MethodInfo)
        //     method; or use the System.Reflection.Emit.ModuleBuilder.GetArrayMethodToken(System.Type,System.String,System.Reflection.CallingConventions,System.Type,System.Type[])
        //     method to get the token directly, instead of using the System.Reflection.Emit.ModuleBuilder.GetArrayMethod(System.Type,System.String,System.Reflection.CallingConventions,System.Type,System.Type[])
        //     method to get a System.Reflection.MethodInfo first.
        int MetadataToken { get; }
        //
        // Summary:
        //     Gets the module in which the type that declares the member represented by the
        //     current System.Reflection.MemberInfo is defined.
        //
        // Returns:
        //     The System.Reflection.Module in which the type that declares the member represented
        //     by the current System.Reflection.MemberInfo is defined.
        //
        // Exceptions:
        //   T:System.NotImplementedException:
        //     This method is not implemented.
        Module Module { get; }
        //
        // Summary:
        //     Returns a value that indicates whether this instance is equal to a specified
        //     object.
        //
        // Parameters:
        //   obj:
        //     An object to compare with this instance, or null.
        //
        // Returns:
        //     true if obj equals the type and value of this instance; otherwise, false.
        bool Equals(object obj);
        //
        // Summary:
        //     When overridden in a derived class, returns an array of all custom attributes
        //     applied to this member.
        //
        // Parameters:
        //   inherit:
        //     true to search this member's inheritance chain to find the attributes; otherwise,
        //     false. This parameter is ignored for properties and events; see Remarks.
        //
        // Returns:
        //     An array that contains all the custom attributes applied to this member, or an
        //     array with zero elements if no attributes are defined.
        //
        // Exceptions:
        //   T:System.InvalidOperationException:
        //     This member belongs to a type that is loaded into the reflection-only context.
        //     See How to: Load Assemblies into the Reflection-Only Context.
        //
        //   T:System.TypeLoadException:
        //     A custom attribute type could not be loaded.
        object[] GetCustomAttributes(bool inherit);
        //
        // Summary:
        //     When overridden in a derived class, returns an array of custom attributes applied
        //     to this member and identified by System.Type.
        //
        // Parameters:
        //   attributeType:
        //     The type of attribute to search for. Only attributes that are assignable to this
        //     type are returned.
        //
        //   inherit:
        //     true to search this member's inheritance chain to find the attributes; otherwise,
        //     false. This parameter is ignored for properties and events; see Remarks.
        //
        // Returns:
        //     An array of custom attributes applied to this member, or an array with zero elements
        //     if no attributes assignable to attributeType have been applied.
        //
        // Exceptions:
        //   T:System.TypeLoadException:
        //     A custom attribute type cannot be loaded.
        //
        //   T:System.ArgumentNullException:
        //     If attributeType is null.
        //
        //   T:System.InvalidOperationException:
        //     This member belongs to a type that is loaded into the reflection-only context.
        //     See How to: Load Assemblies into the Reflection-Only Context.
        object[] GetCustomAttributes(Type attributeType, bool inherit);
        //
        // Summary:
        //     Returns a list of System.Reflection.CustomAttributeData objects representing
        //     data about the attributes that have been applied to the target member.
        //
        // Returns:
        //     A generic list of System.Reflection.CustomAttributeData objects representing
        //     data about the attributes that have been applied to the target member.
        IList<CustomAttributeData> GetCustomAttributesData();
        //
        // Summary:
        //     Returns the hash code for this instance.
        //
        // Returns:
        //     A 32-bit signed integer hash code.
        int GetHashCode();
        //
        // Summary:
        //     When overridden in a derived class, indicates whether one or more attributes
        //     of the specified type or of its derived types is applied to this member.
        //
        // Parameters:
        //   attributeType:
        //     The type of custom attribute to search for. The search includes derived types.
        //
        //   inherit:
        //     true to search this member's inheritance chain to find the attributes; otherwise,
        //     false. This parameter is ignored for properties and events; see Remarks.
        //
        // Returns:
        //     true if one or more instances of attributeType or any of its derived types is
        //     applied to this member; otherwise, false.
        bool IsDefined(Type attributeType, bool inherit);

        //
        // Summary:
        //     When overridden in a derived class, returns the value of a field supported by
        //     a given object.
        //
        // Parameters:
        //   obj:
        //     The object whose field value will be returned.
        //
        // Returns:
        //     An object containing the value of the field reflected by this instance.
        //
        // Exceptions:
        //   T:System.Reflection.TargetException:
        //     In the .NET for Windows Store apps or the Portable Class Library, catch System.Exception
        //     instead. The field is non-static and obj is null.
        //
        //   T:System.NotSupportedException:
        //     A field is marked literal, but the field does not have one of the accepted literal
        //     types.
        //
        //   T:System.FieldAccessException:
        //     In the .NET for Windows Store apps or the Portable Class Library, catch the base
        //     class exception, System.MemberAccessException, instead. The caller does not have
        //     permission to access this field.
        //
        //   T:System.ArgumentException:
        //     The method is neither declared nor inherited by the class of obj.
        object GetValue(object obj);
        //
        // Summary:
        //     Sets the value of the field supported by the given object.
        //
        // Parameters:
        //   obj:
        //     The object whose field value will be set.
        //
        //   value:
        //     The value to assign to the field.
        //
        // Exceptions:
        //   T:System.FieldAccessException:
        //     In the .NET for Windows Store apps or the Portable Class Library, catch the base
        //     class exception, System.MemberAccessException, instead. The caller does not have
        //     permission to access this field.
        //
        //   T:System.Reflection.TargetException:
        //     In the .NET for Windows Store apps or the Portable Class Library, catch System.Exception
        //     instead. The obj parameter is null and the field is an instance field.
        //
        //   T:System.ArgumentException:
        //     The field does not exist on the object.-or- The value parameter cannot be converted
        //     and stored in the field.
        void SetValue(object obj, object value);
        //
        // Summary:
        //     Gets the type of this property / field
        //
        // Returns:
        //     The type of this property / field.
        Type Type { get; }
        //
        // Summary:
        //     Gets the wrapped PropertyInfo / FieldInfo instance
        //
        // Returns:
        //     object type, that can be casted to type PropertyInfo / FieldInfo
        object WrappedInstance { get; }
        //
        // Summary:
        //     Gets an enum indicating the real type of the wrapped FieldInfo / PropertyInfo type
        //
        // Returns:
        //     enum indicating whether the wrapped type is of FieldInfo or PropertyInfo
        PropertyFieldType WrappedType { get; }
    }
}
