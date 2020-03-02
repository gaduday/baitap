using System;

namespace baitap
{

    /// <summary>
    /// Model Hồ sơ, tư liệu
    /// Author: hienvtt
    /// Create date: 2019/02/24
    public class Document
    {
        //public string Id { get; set; }
        //public bool Deleted { get; set; } = false;
        //public DateTime? CreatedDate { get; set; }
        //public DateTime? ModifiedDate { get; set; }
        //public string CreatedUser { get; set; }
        //public string ModifiedUser { get; set; }
        /// <summary>
        /// Mã định danh (Số hồ sơ, tư liệu)
        /// </summary>
        public string Identifier { get; set; }
        /// <summary>
        /// Tên hồ sơ, tư liệu
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Loại tư liệu
        /// </summary>
        public string DocumentType { get; set; }
        /// <summary>
        /// CS: Mã vị trí lưu trữ
        /// </summary>
        public string LocationId { get; set; }
        /// <summary>
        /// CS: Mã đơn vị hành chính
        /// </summary>
        public int DonViHanhChinhId { get; set; }
        /// <summary>
        /// Cộng tác viên
        /// </summary>
        public string Contributor { get; set; }
        /// <summary>
        /// Phạm vi
        /// </summary>
        public string Coverage { get; set; }
        /// <summary>
        /// Tác giả
        /// </summary>
        public string Creator { get; set; }
        /// <summary>
        /// Mã đơn vị lưu trữ, quản lý khai thác
        /// </summary>
        public string DepartmentId { get; set; }
        /// <summary>
        /// Ngày ban hành
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// CS: Kiểu ngày phát hành (0: không có ngày/tháng/năm; 1: Có năm; 2: Có tháng/năm; 3: Có ngày/tháng/năm)
        /// </summary>
        public int DateType { get; set; }
        /// <summary>
        /// Thông tin mô tả mở rộng (Sẽ dùng xml để mô tả chi tiết thêm cho hồ sơ, tư liệu
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Định dạng (Mapping documenttype với type =1)
        /// </summary>
        public string DataType { get; set; }
        /// <summary>
        /// Ngôn ngữ
        /// </summary>
        public string Language { get; set; }
        /// <summary>
        /// Đơn vị xuất bản
        /// </summary>
        public string Publisher { get; set; }
        /// <summary>
        /// Hồ sơ, tư liệu liên quan
        /// </summary>
        public string[] Relations { get; set; }
        /// <summary>
        /// Thông tin về bản quyền
        /// </summary>
        public string Rights { get; set; }
        /// <summary>
        /// Nguồn dữ liệu (tham khảo)
        /// </summary>
        public string Source { get; set; }
        /// <summary>
        /// Chủ đề, từ khoá
        /// </summary>
        public string Subject { get; set; }
        /// <summary>
        /// CS: Số lượng
        /// </summary>
        public int Amount { get; set; } = 1;
        /// <summary>
        /// CS: Đơn vị tính
        /// </summary>
        public string Unit { get; set; } = "Bộ";
        /// <summary>
        /// CS: Độ mật (0: Không mật, 1: Bình thường, 2: Mật, 3: Tối mật)
        /// </summary>
        public int SecurityLevel { get; set; } = 0;
        /// <summary>
        /// CS: Miễn phí/Mất phí khi khai thác
        /// </summary>
        public bool IsFree { get; set; } = false;
        /// <summary>
        /// CS: Mã ứng dụng
        /// </summary>
        public string AppId { get; set; } = "";
        /// <summary>
        /// CS: Danh sách tài liệu đính kèm
        /// </summary>
        public Component[] Components { get; set; }
        /// <summary>
        /// CS: Danh sách file của hồ sơ, tư liệu
        /// </summary>
        public File[] Files { get; set; }
    }

    /// <summary>
    /// Model tài liệu đính kèm
    /// Author: tuanva
    /// Create date: 2019/11/27
    /// </summary>
    public class Component
    {
        public string Id { get; set; }
        public bool Deleted { get; set; } = false;
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string CreatedUser { get; set; }
        public string ModifiedUser { get; set; }
        /// <summary>
        /// Tên tài liệu
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Số hiệu
        /// </summary>
        public string Identifier { get; set; }
        /// <summary>
        /// Tác giả
        /// </summary>
        public string Creator { get; set; }
        /// <summary>
        /// Ngày ban hành
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// Kiểu ngày phát hành (0: không có ngày/tháng/năm; 1: Có năm; 2: Có tháng/năm; 3: Có ngày/tháng/năm)
        /// </summary>
        public int DateType { get; set; }
        /// <summary>
        /// Mô tả
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Trích yếu
        /// </summary>
        public string Abstract { get; set; }
        /// <summary>
        /// Hình thức văn bản (Bản chính, photo, scan, công chứng, viết tay)
        /// </summary>
        public string Documentform { get; set; } = "Bản chính";
        /// <summary>
        /// Số lượng
        /// </summary>
        public int Amount { get; set; } = 1;
        /// <summary>
        /// Đơn vị tính
        /// </summary>
        public string Unit { get; set; } = "Tờ";
        /// <summary>
        /// Tờ số
        /// </summary>
        public string Order { get; set; }
        /// <summary>
        /// Danh sách file của tài liệu đính kèm
        /// </summary>
        public File[] Files { get; set; }
    }

    /// <summary>
    /// Model File data
    /// Author: tuanva
    /// Create date: 2019/11/27
    /// </summary>
    public class File
    {
        /// <summary>
        /// Tên file
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Format file
        /// </summary>
        public string Extension { get; set; }
        /// <summary>
        /// Tổng số trang
        /// </summary>
        public int PageCount { get; set; } = 0;
        /// <summary>
        /// Dung lượng file
        /// </summary>
        public string Size { get; set; }
        /// <summary>
        /// Dữ liệu file
        /// </summary>
        public string Data { get; set; }
        /// <summary>
        /// Kiểu lưu trữ. Quy định (0: BLOB; 1: FTP; 2: File Share)
        /// </summary>
        public int StoreType { get; set; } = 1;
    }
}
