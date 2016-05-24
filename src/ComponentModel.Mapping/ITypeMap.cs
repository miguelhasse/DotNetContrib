using System;
using System.Linq.Expressions;

namespace Hasseware.ComponentModel.Mapping
{
    public interface ITypeMap<TFrom, TTo>
    {
        void MapGetIndexer<TFromMember>(Expression<Func<TFrom, string, TFromMember>> fromIndexer);

        void MapSetIndexer<TToMember>(Expression<Action<TTo, string, TToMember>> toIndexer);

        void Map<TFromMember, TToMember>(Expression<Func<TFrom, TFromMember>> from, Expression<Action<TTo, TToMember>> to);

        void Map<TFromMember, TToMember>(Expression<Func<TFrom, TFromMember>> from, Expression<Func<TTo, TToMember>> to);

        void Map<TFromMember, TToMember>(Expression<Func<TFrom, TFromMember>> from, string toName);

        void Map<TFromMember, TToMember>(string fromName, Expression<Action<TTo, TToMember>> to);

        void Map<TFromMember, TToMember>(string fromName, Expression<Func<TTo, TToMember>> to);

        void Map(string fromName, string toName);
    }
}