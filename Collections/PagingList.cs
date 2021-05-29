using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Qrame.CoreFX.Collections
{
    /// <summary>
    /// 페이징을 지원하는 List 타입의 확장 클래스입니다.
    /// </summary>
    /// <typeparam name="T">페이징을 수행하는 데이터 타입입니다.</typeparam>
    public class PagingList<T> : List<T>, IPagingList
    {
        /// <summary>
        /// 컬렉션의 총 개수를 가져오거나 설정합니다.
        /// </summary>
        public int TotalCount
        {
            get;
            set;
        }

        /// <summary>
        /// 컬렉션의 총 페이지수를 가져오거나 설정합니다.
        /// </summary>
        public int TotalPages
        {
            get;
            set;
        }

        /// <summary>
        /// 컬렉션의 지정된 페이지를 가져오거나 설정합니다.
        /// </summary>
        public int PageIndex
        {
            get;
            set;
        }

        /// <summary>
        /// 컬렉션의 페이지 사이즈를 가져오거나 설정합니다.
        /// </summary>
        public int PageSize
        {
            get;
            set;
        }

        /// <summary>
        /// 컬렉션에서 이전 페이지를 가져올 수 있는지 확인합니다.
        /// </summary>
        public bool IsPreviousPage
        {
            get
            {
                return (PageIndex > 0);
            }
        }

        /// <summary>
        /// 컬렉션에서 다음 페이지를 가져올 수 있는지 확인합니다.
        /// </summary>
        public bool IsNextPage
        {
            get
            {
                return (PageIndex * PageSize) <= TotalCount;
            }
        }

        /// <summary>
        /// 인스턴스 생성시, 페이징 값을 초기화합니다.
        /// </summary>
        /// <param name="dataSource">데이터 형식이 알려진 특정 데이터 소스에 대한 쿼리를 실행하는 기능을 제공하는 타입입니다.</param>
        /// <param name="index">페이징 하고자 하는 페이지 인덱스 값입니다.</param>
        /// <param name="pageSize">전체 페이지 사이즈 값입니다.</param>
        public PagingList(IQueryable<T> dataSource, int index, int pageSize)
        {
            int dataCount = dataSource.Count();

            this.TotalCount = dataCount;
            this.TotalPages = dataCount / pageSize;

            if (dataCount % pageSize > 0)
            {
                TotalPages++;
            }

            this.PageSize = pageSize;
            this.PageIndex = index;
            this.AddRange(dataSource.Skip(index * pageSize).Take(pageSize).ToList());
        }

        /// <summary>
        /// 인스턴스 생성시, 페이징 값을 초기화합니다.
        /// </summary>
        /// <param name="dataSource">인덱스로 액세스할 수 있는 강력한 형식의 개체 목록을 나타냅니다.</param>
        /// <param name="index">페이징 하고자 하는 페이지 인덱스 값입니다.</param>
        /// <param name="pageSize">전체 페이지 사이즈 값입니다.</param>
        public PagingList(List<T> dataSource, int index, int pageSize)
        {

            int dataCount = dataSource.Count();

            this.TotalCount = dataCount;
            this.TotalPages = dataCount / pageSize;

            if (dataCount % pageSize > 0)
            {
                TotalPages++;
            }

            this.PageSize = pageSize;
            this.PageIndex = index;
            this.AddRange(dataSource.Skip(index * pageSize).Take(pageSize).ToList());
        }
    }

    /// <summary>
    /// 결과내 재검색을 위한 페이징을 지원하는 List 타입의 확장 클래스입니다.
    /// </summary>
    public static class Pagination
    {
        /// <summary>
        /// 페이징을 지원하는 List 타입의 확장 클래스를 반환합니다.
        /// </summary>
        /// <typeparam name="T">페이징을 수행하는 데이터 타입입니다.</typeparam>
        /// <param name="dataSource">데이터 형식이 알려진 특정 데이터 소스에 대한 쿼리를 실행하는 기능을 제공하는 타입입니다.</param>
        /// <param name="index">페이징 하고자 하는 페이지 인덱스 값입니다.</param>
        /// <param name="pageSize">전체 페이지 사이즈 값입니다.</param>
        /// <returns>페이징을 지원하는 List 타입의 확장 클래스입니다.</returns>
        public static PagingList<T> ToPagedList<T>(this IQueryable<T> dataSource, int index, int pageSize)
        {
            return new PagingList<T>(dataSource, index, pageSize);
        }

        /// <summary>
        /// 페이징을 지원하는 List 타입의 확장 클래스를 반환합니다.
        /// </summary>
        /// <typeparam name="T">페이징을 수행하는 데이터 타입입니다.</typeparam>
        /// <param name="dataSource">데이터 형식이 알려진 특정 데이터 소스에 대한 쿼리를 실행하는 기능을 제공하는 타입입니다.</param>
        /// <param name="index">페이징 하고자 하는 페이지 인덱스 값입니다.</param>
        /// <returns>페이징을 지원하는 List 타입의 확장 클래스입니다.</returns>
        public static PagingList<T> ToPagedList<T>(this IQueryable<T> dataSource, int index)
        {
            return new PagingList<T>(dataSource, index, 10);
        }
    }
}
