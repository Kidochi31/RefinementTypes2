using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefinementTypes2.Typing
{
    internal abstract class Type
    {
        public abstract bool WillBeSubtypeOf(NamedType type);

        public bool MayBeSubtypeOf(NamedType type) => !WillBeSubtypeOf(type) && !WontBeSubtypeOf(type);

        public abstract bool WontBeSubtypeOf(NamedType type);
    }

    internal class NamedType : Type
    {
        public string Name { get; set; }
        public Type BaseType { get; set; }

        private static NamedType CreateAnyType()
        {
            NamedType anyType = new NamedType("Any");
            anyType.BaseType = anyType;
            return anyType;
        }
        public NamedType(string name)
        {
            Name = name;
            BaseType = Any;
        }

        public NamedType(string name, Type baseType)
        {
            Name = name;
            BaseType = baseType;
        }

        public static NamedType Any = CreateAnyType();
        public static NamedType Type = new NamedType("Type");

        public override bool WillBeSubtypeOf(NamedType type)
        {
            if (Equals(type))
                return true;
            if (Equals(Any))
                return false;
            return BaseType.WillBeSubtypeOf(type);

        }

        public override bool WontBeSubtypeOf(NamedType type)
        {
            if (Equals(type))
                return false;
            if (Equals(Any))
                return true;
            return BaseType.WontBeSubtypeOf(type);
        }
    }

    internal class RefinedType : Type
    {
        public Type BaseType;
        public Refinement Refinement;

        public RefinedType(Type baseType, Refinement refinement)
        {
            BaseType = baseType;
            Refinement = refinement;
        }

        public override bool WillBeSubtypeOf(NamedType type) => BaseType.WillBeSubtypeOf(type);

        public override bool WontBeSubtypeOf(NamedType type) => BaseType.WontBeSubtypeOf(type);
    }

    internal class OrType : Type
    {
        public List<Type> BaseTypes;

        public OrType(List<Type> baseTypes)
        {
            BaseTypes = baseTypes;
        }

        public override bool WillBeSubtypeOf(NamedType type) => BaseTypes.All(baseType => baseType.WillBeSubtypeOf(type));

        public override bool WontBeSubtypeOf(NamedType type) => BaseTypes.All(baseType => baseType.WontBeSubtypeOf(type));
    }

    internal class AndType : Type
    {
        public List<Type> BaseTypes;

        public AndType(List<Type> baseTypes)
        {
            BaseTypes = baseTypes;
        }

        public override bool WillBeSubtypeOf(NamedType type) => BaseTypes.Any(baseType => baseType.WillBeSubtypeOf(type));

        public override bool WontBeSubtypeOf(NamedType type) => BaseTypes.Any(baseType => baseType.WontBeSubtypeOf(type));
    }
}
