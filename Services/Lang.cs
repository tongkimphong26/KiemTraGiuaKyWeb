using Microsoft.AspNetCore.Mvc.ModelBinding;

public class Lang
{
    private readonly IHttpContextAccessor _h;
    public Lang(IHttpContextAccessor h) { _h = h; }

    // Mặc định English, chỉ là tiếng Việt khi cookie lang = vi
    public string Current => _h.HttpContext?.Request.Cookies["lang"] == "vi" ? "vi" : "en";

    static readonly Dictionary<string, (string en, string vi)> D = new()
    {
        ["AppName"] = ("Dish Management", "Quản lý món ăn"),
        ["Dishes"] = ("Dishes", "Món ăn"),
        ["Categories"] = ("Categories", "Loại món"),
        ["Create"] = ("Create", "Thêm mới"),
        ["Edit"] = ("Edit", "Sửa"),
        ["Delete"] = ("Delete", "Xóa"),
        ["Details"] = ("Details", "Chi tiết"),
        ["Back"] = ("Back", "Quay lại"),
        ["Save"] = ("Save", "Lưu"),
        ["Search"] = ("Search by name", "Tìm theo tên món"),
        ["Filter"] = ("Filter", "Lọc"),
        ["Reset"] = ("Reset", "Đặt lại"),
        ["Name"] = ("Dish name", "Tên món"),
        ["CatName"] = ("Category name", "Tên loại món"),
        ["Price"] = ("Price", "Giá"),
        ["Prep"] = ("Preparation time (min)", "Thời gian chế biến (phút)"),
        ["Category"] = ("Category", "Loại món"),
        ["Status"] = ("Status", "Trạng thái"),
        ["Available"] = ("Available", "Đang bán"),
        ["Unavailable"] = ("Unavailable", "Ngừng bán"),
        ["All"] = ("All", "Tất cả"),
        ["Quick"] = ("Quick", "Nhanh"),
        ["Regular"] = ("Regular", "Thông thường"),
        ["MinPrice"] = ("Min price", "Giá từ"),
        ["MaxPrice"] = ("Max price", "Giá đến"),
        ["Sort"] = ("Sort", "Sắp xếp"),
        ["PriceAsc"] = ("Price ascending", "Giá tăng dần"),
        ["PriceDesc"] = ("Price descending", "Giá giảm dần"),
        ["PrepAsc"] = ("Prep time ascending", "Thời gian chế biến tăng dần"),
        ["NoResult"] = ("No dishes found", "Không tìm thấy món ăn nào"),
        ["NoImage"] = ("No image", "Chưa có ảnh"),
        ["Images"] = ("Images", "Hình ảnh"),
        ["ImageUrl"] = ("Image URL", "Đường dẫn ảnh"),
        ["Thumbnail"] = ("Thumbnail", "Ảnh đại diện"),
        ["SetThumb"] = ("Set as thumbnail", "Chọn làm ảnh đại diện"),
        ["AddImage"] = ("Add image", "Thêm ảnh"),
        ["Description"] = ("Description", "Mô tả"),
        ["DishCount"] = ("Dish count", "Số món"),
        ["CreateDish"] = ("Create dish", "Thêm món ăn"),
        ["EditDish"] = ("Edit dish", "Sửa món ăn"),
        ["DishDetails"] = ("Dish details", "Chi tiết món ăn"),
        ["CreateCategory"] = ("Create category", "Thêm loại món"),
        ["EditCategory"] = ("Edit category", "Sửa loại món"),
        ["Err_Required"] = ("This field is required", "Không được để trống"),
        ["Err_Price"] = ("Price must be greater than 0", "Giá phải lớn hơn 0"),
        ["Err_Prep"] = ("Preparation time must be greater than 0", "Thời gian chế biến phải lớn hơn 0"),
        ["Err_Len"] = ("Value is too long", "Giá trị quá dài"),
        ["Err_Invalid"] = ("Invalid value", "Giá trị không hợp lệ"),
        ["Err_Dup"] = ("Dish name already exists in this category", "Tên món đã tồn tại trong loại này"),
        ["Err_Cat"] = ("Category does not exist", "Loại món không tồn tại"),
        ["Err_Range"] = ("Invalid price range", "Khoảng giá không hợp lệ"),
        ["Err_NotFound"] = ("Not found", "Không tìm thấy"),
        ["Err_CatInUse"] = ("Cannot delete: category is in use", "Không thể xóa: loại món đang được sử dụng"),
    };

    public string this[string key] =>
        D.TryGetValue(key, out var v) ? (Current == "vi" ? v.vi : v.en) : key;

    // Dịch thông báo validation (gọi 1 lần duy nhất cho mỗi request)
    public void Translate(ModelStateDictionary ms)
    {
        foreach (var e in ms.Values)
        {
            if (e.Errors.Count == 0) continue;
            var msgs = e.Errors.Select(x => x.ErrorMessage).ToList();
            e.Errors.Clear();
            foreach (var m in msgs)
                e.Errors.Add(D.ContainsKey(m) ? this[m] : this["Err_Invalid"]);
        }
    }
}