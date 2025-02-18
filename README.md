# Project Title : Gyminize


## 1. Mô tả project
### Project này là ứng dụng WinUI dành cho người tập gym, có các chức năng như theo dõi lượng calo từ thức ăn, quá trình tập luyện, lên plan luyện tập theo từng cá nhân. Ngoài ra người sử dụng còn có thể shopping để mua các thực phẩm bổ sung và dụng cụ hỗ trợ tập luyện.


## 2. Design Patterns / Architecture (20%)

- Sử dụng kiến trúc MVVM, code xử lí thông qua chủ yếu View Model
- Tổng hợp các design patterns:
  - Dependency Injection và Singleton(DialogService.cs, ApiServicesCLient.cs, WindowService.cs,..) : các phụ thuộc được tiêm vào qua constructor, giúp tách biệt khởi tạo đối tượng và logic nghiệp vụ
  - Command Design Pattern: Sử dụng RelayCommand từ CommunityToolkit.Mvvm.Input để thực hiện các hành động trong các ViewModel.
  - Observer Pattern: sử dụng để thông báo cho các đối tượng quan sát khi có thay đổi trong đối tượng được quan sát, sử dụng ở hầu hết các viewmodel phục vụ binding
  - Repository Design Pattern: dựa vào ApiServicesCLient lấy dữ liệu gián tiếp thông qua api (thêm các repository khác phục vụ chức năng thanh toán PaymentRepository.cs)

## 3. Tổng Quan & Các Tính Năng ChínhMarkdown All in One
  - Theo dõi calo và lịch trình tập luyện của người dùng.
  - Lên kế hoạch tập luyện và ăn uống.
  - Chatbot hỗ trợ hỏi đáp.
  - Shop mua bán thực phẩm bổ sung, khách hàng có thể xem lại lịch sử mua sắm.
  - Design Patterns.
  - Oauth Google để đăng nhập.
  - Email sende
  - Sử dụng API RestAPI
  - Hỗ trợ backup và restore qua pg_dumb và pg_restore, lập lịch thủ công (xem README trong thư mục Database)
  - Hỗ trợ thanh toán qua dịch vụ VNPAY.
  - Unit test.


## 4. Testing và Document
- Unit test ở project Gyminize.MSTest test cho 4 ViewModel tương ứng 4 màn hình chức năng chính (Home, Nutrition, Plan, Diary) tổng cộng 33 test, Xem Test Document trong thư mục Document
- Thông tin về các lớp chính ở ViewModel, API, Services và Test được trình bày trong file Doxygen của thư mục Document, cấu hình lại đường dẫn, run Doxygen và chạy index.html để xem thông tin project

## Hướng dẫn build chương trình

1. Người dùng vào solution của ứng dụng, click chuột phải vào Solution **WINUI** -> Properties -> Multiple startup projects -> Chọn Action cho Project **Gyminize** và **Gyminize_API** là **Start** -> OK.
   Việc này để khởi chạy project API và Project Gyminize cùng lúc.
2. Nhấn **CTRL+F5** để chạy chương trình

# Video demo [Youtube](https://www.youtube.com/watch?v=kmXx-rZYqSw)
