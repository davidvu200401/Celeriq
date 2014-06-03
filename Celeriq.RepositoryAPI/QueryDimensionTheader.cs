#pragma warning disable 0168
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Celeriq.Common;

namespace Celeriq.RepositoryAPI
{
    internal class QueryDimensionTheader
    {
        private DataQueryResults _newResults = null;
        private IEnumerable<DataItemExtension> _queriedList = null;
        private List<DimensionItem> _dimensionList = null;
        private RepositorySchema _definition = null;
        private int _dimensionIndex = 0;

        public QueryDimensionTheader(DataQueryResults newResults, IEnumerable<DataItemExtension> queriedList, List<DimensionItem> dimensionList, RepositorySchema definition, int dimensionIndex)
        {
            _newResults = newResults;
            _queriedList = queriedList;
            _dimensionList = dimensionList;
            _definition = definition;
            _dimensionIndex = dimensionIndex;
        }

        public void ProcessDimension()
        {
            var debugInt = 0;
            try
            {
                var dimension = _dimensionList[_dimensionIndex];
                debugInt = 1;
                var dimensionDef = _definition.DimensionList.First(x => x.Name == dimension.Name);
                debugInt = 2;
                var rIDList = dimension.RefinementList.Select(x => x.DVIdx);
                debugInt = 3;
                if (_newResults.Query.DimensionValueList != null)
                {
                    debugInt = 4;
                    if (_newResults.Query.DimensionValueList.Any(x => rIDList.Contains(x)))
                    {
                        lock (_newResults)
                        {
                            debugInt = 5;
                            _newResults.DimensionList.Add(new DimensionItem() { Name = dimension.Name, DIdx = dimension.DIdx, NumericBreak = dimension.NumericBreak });
                        }
                        return;
                    }
                }

                debugInt = 6;
                var d1 = new DimensionItem() { Name = dimension.Name, DIdx = dimension.DIdx, NumericBreak = dimension.NumericBreak };
                debugInt = 7;

                #region Lists

                if (dimensionDef.DataType == RepositorySchema.DataTypeConstants.List)
                {
                    foreach (var item in _queriedList)
                    {
                        var dvidxBase = ((d1.DIdx - DimensionDefinition.DGROUP) + 1) * DimensionDefinition.DVALUEGROUP;
                        foreach (var dvidx in item.DimensionValueArray.Where(x => dvidxBase <= x && x < (dvidxBase + DimensionDefinition.DVALUEGROUP)))
                        {
                            var rItem = d1.RefinementList.FirstOrDefault(x => x.DVIdx == dvidx);
                            if (rItem == null)
                            {
                                rItem = ((ICloneable)dimension.RefinementList.First(x => x.DVIdx == dvidx)).Clone() as RefinementItem;
                                if (rItem != null)
                                {
                                    rItem.Count = 1;
                                    d1.RefinementList.Add(rItem);
                                }
                            }
                            else
                            {
                                rItem.Count++;
                            }

                        }
                    }
                }
                #endregion

                #region Strings, Dates, Numbers (with no bands)

                else if (d1.NumericBreak == null)
                {
                    var r1 = (from x in _queriedList
                              where x.DimensionSingularValueArray[_dimensionIndex] != null
                              group x by x.DimensionSingularValueArray[_dimensionIndex]
                                  into g
                                  select new { Key = g.Key, Count = g.Count() }).ToList();

                    var refinementCache = new Utilities.HashTable<long?, RefinementItem>();
                    foreach (var r in dimension.RefinementList)
                        refinementCache.Add(r.DVIdx, r);

                    foreach (var r in r1)
                    {
                        var refinement = new RefinementItem();
                        refinement.DVIdx = r.Key.Value;
                        //refinement.FieldValue = dimension.RefinementList.First(x => x.DVIdx == r.Key).FieldValue;
                        refinement.FieldValue = refinementCache[r.Key].FieldValue;
                        refinement.Count = r.Count;
                        if ((dimensionDef.DataType == RepositorySchema.DataTypeConstants.Float) || (dimensionDef.DataType == RepositorySchema.DataTypeConstants.Int))
                        {
                            refinement.MinValue = long.Parse(refinement.FieldValue);
                            refinement.MaxValue = refinement.MinValue;
                        }
                        d1.RefinementList.Add(refinement);
                    }
                }
                #endregion

                #region Ints with bands

                else //with numeric bands
                {
                    var r1 = (from x in _queriedList
                              where x.DimensionSingularValueArray[_dimensionIndex] != null
                              group x by x.DimensionSingularValueArray[_dimensionIndex]
                                  into g
                                  select new { Key = g.Key, Count = g.Count() }).ToList();

                    foreach (var r in r1)
                    {
                        var rItem = dimension.RefinementList.FirstOrDefault(x => x.DVIdx == r.Key);
                        if (rItem != null)
                        {
                            var refinement = new RefinementItem();
                            refinement.DVIdx = r.Key.Value;
                            refinement.FieldValue = rItem.FieldValue;
                            refinement.Count = r.Count;
                            refinement.MinValue = rItem.MinValue;
                            refinement.MaxValue = rItem.MaxValue;
                            d1.RefinementList.Add(refinement);
                        }
                    }
                }

                #endregion

                //Do not add dimensions with [0..1] items
                //if (d1.RefinementList.Count > 1)

                lock (_newResults)
                {
                    _newResults.DimensionList.Add(d1);
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}