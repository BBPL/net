//using Microsoft.Extensions.Caching.Memory;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection;
//using System.Text;
//using System.Threading.Tasks;

//namespace Shape.Weather.Common.Cache
//{
//    internal class CacheExtensions
//    {
//        /// <summary>
//        /// From cache removes all items connected to specified documentId
//        /// It will only remove items connected by keys in InternEntryKeys class
//        /// </summary>
//        /// <param name="cache"></param>
//        /// <param name="documentId"></param>
//        public static void RemoveRelatedToDocument(this IMemoryCache cache, Guid documentId)
//        {
//            RemoveRelated(cache, typeof(InternEntryKeys.DocumentRelated), documentId);
//        }

//        /// <summary>
//        /// From cache removes all items connected to specified caseId.
//        /// It will only remove items connected by keys in InternEntryKeys class
//        /// </summary>
//        /// <param name="cache"></param>
//        /// <param name="caseId"></param>
//        public static void RemoveRelatedToCase(this IMemoryCache cache, Guid caseId)
//        {
//            RemoveRelated(cache, typeof(InternEntryKeys.CaseRelated), caseId);
//        }

//        /// <summary>
//        /// This method is responsible for removing affected entries based on the input from anonymization state update DTO
//        /// </summary>
//        /// <param name="cache"></param>
//        /// <param name="updateDto"></param>
//        public static void HandleCachedAnonymizationState(this IMemoryCache cache, AnonymizationStateUpdateDTO updateDto)
//        {
//            updateDto.Documents.ForEach(docId =>
//                cache.Remove(InternEntryKeys.DocumentRelated.MarkingsInDocument(docId))
//            );

//            if (updateDto.CaseId != Guid.Empty)
//            {
//                var caseId = updateDto.CaseId;
//                cache.Remove(InternEntryKeys.CaseRelated.IdentificationGroupsInCase(caseId));
//                cache.Remove(InternEntryKeys.CaseRelated.GetSubstitutionsArk(caseId));
//            }
//        }

//        /// <summary>
//        /// Scan relevant InternEntryKeys subclass to re-create entry keys and then remove them from cache
//        /// </summary>
//        /// <param name="cache"></param>
//        /// <param name="type"></param>
//        /// <param name="id"></param>
//        private static void RemoveRelated(IMemoryCache cache, Type type, Guid id)
//        {
//            var keyList = type
//                .GetMethods(BindingFlags.Static | BindingFlags.Public)
//                .Select(x => x.Invoke(null, new object[] { id }))
//                .Where(x => x != null)
//                .Select(x => x.ToString())
//                .ToList();

//            keyList.ForEach(x => cache.Remove(x));
//        }
//    }
//}
