# VuHongPhong-24810320183
I. PHẦN LÝ THUYẾT & CÂU HỎI NGẮN 

PNG 

Câu 1: Trình bày sự khác nhau giữa Value Types (Kiểu giá trị) và Reference Types (Kiểu tham chiếu) trong C# về cơ chế lưu trữ vùng nhớ (Stack vs Heap). 

PNG 

Cơ chế lưu trữ vùng nhớ: 

Value Types (Kiểu giá trị): Dữ liệu được lưu trữ trực tiếp tại nơi biến được khai báo (thường nằm trên Stack nếu là biến cục bộ trong phương thức). Khi biến vượt ra khỏi phạm vi (scope), vùng nhớ sẽ tự động được giải phóng ngay lập tức.  

PNG+ 1 

Reference Types (Kiểu tham chiếu): Biến nằm trên Stack thực chất chỉ chứa một địa chỉ (tham chiếu) dẫn đến vùng dữ liệu thực sự nằm trên Heap. Vùng nhớ trên Heap sẽ được tự động thu hồi bởi bộ thu gom rác (Garbage Collector - GC) khi không còn bất kỳ tham chiếu nào trỏ tới nó.  

PNG+ 1 

Hành vi sao chép dữ liệu: 

Value Types: Khi gán biến này cho biến khác, toàn bộ giá trị được sao chép sang một ô nhớ mới hoàn toàn độc lập.  

PNG 

Reference Types: Chỉ có địa chỉ tham chiếu bị sao chép, do đó cả hai biến cùng trỏ chung đến một đối tượng duy nhất trên Heap. Thay đổi dữ liệu qua một biến sẽ ảnh hưởng trực tiếp đến biến còn lại.  

PNG 

Ví dụ minh họa bằng C#: 

C# 

// 1. Value Type (int) 
int a = 10; 
int b = a; // b nhận bản sao giá trị 10 
b = 20;    // a vẫn giữ nguyên giá trị 10 
Console.WriteLine($"a = {a}, b = {b}"); // Kết quả: a = 10, b = 20 
 
// 2. Reference Type (class) 
class Student { public string Name; } 
Student s1 = new Student { Name = "An" }; 
Student s2 = s1; // s2 trỏ cùng đến vùng nhớ với s1 trên Heap 
s2.Name = "Bình"; // Thay đổi qua s2 làm thay đổi luôn s1 
Console.WriteLine($"s1 = {s1.Name}, s2 = {s2.Name}"); // Kết quả: s1 = Bình, s2 = Bình 
 

Câu 2: Tính năng Init-only Properties (init) trong C# 9/10 khác gì so với thuộc tính có set thông thường? Nêu trường hợp sử dụng thực tế. 

PNG 

Sự khác biệt: 

Thuộc tính có set thông thường: Cho phép gán và thay đổi giá trị của thuộc tính bất cứ lúc nào trong suốt vòng đời của đối tượng (mutable).  

PNG 

Thuộc tính init (Init-only): Chỉ cho phép gán giá trị một lần duy nhất trong quá trình khởi tạo đối tượng (thông qua bộ khởi tạo đối tượng - object initializer hoặc constructor). Sau khi khởi tạo xong, thuộc tính tự động chuyển sang trạng thái chỉ đọc (read-only), không thể thay đổi giá trị từ bên ngoài nữa.  

PNG 

Trường hợp sử dụng thực tế: 

Thường được dùng để thiết kế các đối tượng bất biến (Immutable Objects), các DTO (Data Transfer Objects), hoặc cấu hình hệ thống. Giúp đảm bảo tính toàn vẹn dữ liệu, tránh việc vô tình làm thay đổi trạng thái của đối tượng sau khi đã thiết lập xong.  

PNG+ 1 

Ví dụ minh họa bằng C#: 

C# 

public class TransactionDto 
{ 
   public string TransactionId { get; init; } // Chỉ gán được lúc khởi tạo 
   public decimal Amount { get; set; }        // Có thể thay đổi thoải mái 
} 
 
// Sử dụng: 
var tx = new TransactionDto  
{  
   TransactionId = "TX123",  
   Amount = 500.0m  
}; 
 
// tx.TransactionId = "TX999"; // Lỗi biên dịch (Compile-time error)! Không thể gán lại. 
tx.Amount = 750.0m;            // Hợp lệ vì dùng 'set' 
 

Câu 3: Phân tích sự khác nhau giữa phương thức virtual ở lớp cha và phương thức override ở lớp con khi triển khai tính Đa hình (Polymorphism). 

PNG 

Phương thức virtual (ở lớp cha): 

Dùng để khai báo một phương thức mà lớp cha cung cấp sẵn một phần cài đặt (implementation) mặc định, đồng thời cho phép các lớp con được quyền ghi đè (override) lại hành vi đó nếu muốn thay đổi hoặc mở rộng.  

PNG 

Phương thức override (ở lớp con): 

Dùng ở lớp con để viết đè hoặc thay thế hoàn toàn phương thức virtual (hoặc abstract) đã được định nghĩa ở lớp cha.  

PNG 

Cơ chế Đa hình: Khi gọi phương thức thông qua biến tham chiếu kiểu lớp cha nhưng trỏ đến thể hiện của lớp con, chương trình sẽ tự động thực thi phiên bản phương thức đã bị override ở lớp con tại thời điểm chạy (Runtime/Dynamic Binding).  

PNG 

Ví dụ minh họa bằng C#: 

C# 

public class Animal 
{ 
   public virtual void MakeSound() // Cho phép lớp con ghi đè 
   { 
       Console.WriteLine("Động vật phát ra âm thanh chung chung"); 
   } 
} 
 
public class Cat : Animal 
{ 
   public override void MakeSound() // Ghi đè phương thức của lớp cha 
   { 
       Console.WriteLine("Meo meo!"); 
   } 
} 
 
// Kiểm chứng tính đa hình: 
Animal myCat = new Cat(); // Tham chiếu lớp cha trỏ đến đối tượng lớp con 
myCat.MakeSound(); // Kết quả: "Meo meo!" (chạy phương thức đã override ở lớp con) 
 

Câu 4: Tại sao một thành phần được khai báo là static trong Lớp (Class) lại không thể truy xuất thông qua một thể hiện (Object Instance) được tạo bằng toán tử new? 

PNG 

Nguyên nhân: 

Các thành phần static (thuộc tính hoặc phương thức tĩnh) thuộc về cấp độ của Lớp (Class-level). Chúng được cấp phát bộ nhớ cố định một lần duy nhất khi lớp được nạp vào AppDomain và tồn tại độc lập với mọi đối tượng.  

PNG 

Trong khi đó, toán tử new dùng để tạo ra các thể hiện cụ thể (Object Instance) nằm trên Heap, mang các dữ liệu riêng biệt của từng object đó. Các object instance không mang theo các bản sao của thành phần static.  

PNG+ 1 

Ngôn ngữ C# ngăn cản việc gọi static qua biến instance nhằm tránh sự nhầm lẫn về mặt ngữ nghĩa và ép buộc lập trình viên phải gọi trực tiếp thông qua Tên Lớp.  

PNG 

Ví dụ minh họa bằng C#: 

C# 

public class Configuration 
{ 
   public static string AppName = "C# Core App"; // Thuộc tính static 
   public int SessionId = 123;                   // Thuộc tính instance thông thường 
} 
 
// Cách truy xuất ĐÚNG: 
string name = Configuration.AppName; // Truy xuất trực tiếp thông qua tên Lớp 
 
// Cách truy xuất SAI (Gây lỗi biên dịch): 
Configuration config = new Configuration(); 
// string wrongName = config.AppName; // Lỗi biên dịch: Không thể truy cập static member qua instance! 
 

 
