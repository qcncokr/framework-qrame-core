namespace Qrame.CoreFX.Collections
{
    /// <summary>
    /// 페이징을 지원하기 위한 컬렉션 인터페이스 정의입니다.
    /// </summary>
    public interface IPagingList
    {
        /// <summary>
        /// 컬렉션의 총 개수를 가져오거나 설정합니다.
        /// </summary>
        int TotalCount
        {
            get;
            set;
        }

        /// <summary>
        /// 컬렉션의 총 페이지수를 가져오거나 설정합니다.
        /// </summary>
        int TotalPages
        {
            get;
            set;
        }

        /// <summary>
        /// 컬렉션의 지정된 페이지를 가져오거나 설정합니다.
        /// </summary>
        int PageIndex
        {
            get;
            set;
        }

        /// <summary>
        /// 컬렉션의 페이지 사이즈를 가져오거나 설정합니다.
        /// </summary>
        int PageSize
        {
            get;
            set;
        }

        /// <summary>
        /// 컬렉션에서 이전 페이지를 가져올 수 있는지 확인합니다.
        /// </summary>
        bool IsPreviousPage
        {
            get;
        }

        /// <summary>
        /// 컬렉션에서 다음 페이지를 가져올 수 있는지 확인합니다.
        /// </summary>
        bool IsNextPage
        {
            get;
        }
    }
}
