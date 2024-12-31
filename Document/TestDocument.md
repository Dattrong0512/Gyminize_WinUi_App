# Tài liệu kiểm thử unit test

## 1. Mục tiêu kiểm thử (Test Objectives)

- Đảm bảo rằng các chức năng ứng dụng hoạt động đúng theo yêu cầu kỹ thuật
- Phát hiện các lỗi trong giai đoạn phát triển để giảm thiểu chi phí sửa chữa
- Đảm bảo chất lượng đầu ra của sản phẩm

## 2. Phạm vi kiểm thử (Test Scope)

- Tập trung vào kiểm tra 4 View Model chính của ứng dụng
- HomeViewModel
- NutritionViewModel
- PlanViewModel
- DiaryViewModel

## 3. Công cụ và môi trường (Test Tools and Environments)

- Công cụ kiểm thử
  - Framework: MSTest và Moq
  - Báo cáo: MarkDown
- Môi trường kiểm thử
  - Hệ điều hành: Windows 10
  - Ngôn ngữ lập trình: C# 12.0.
  - Phụ thuộc khác: .NET 8, Microsoft.UI.Xaml.

## 4. Kế hoạch kiểm thử (Test Plan)

- Lịch trình:
  - Ngày bắt đầu: 24/11/2024.
  - Ngày kết thúc: 30/11/2024.
- Chiến lược:
  - Mỗi lớp (class) sẽ được kiểm tra các phương thức (method) chính.
  - Xử lý các trường hợp:
    - Đầu vào hợp lệ (valid inputs).
    - Đầu vào không hợp lệ (invalid inputs).
    - Các tình huống ngoại lệ (exception cases).

## 5. Chi tiết kiểm thử (Test Details)

### 5.1 Home View Model

#### 5.1.1 `OpenSaveWeight_ShouldShowErrorDialog_WhenWeightIsNotNumeric`

- **ID**: TC01
- **Tên**: Kiểm tra thông báo lỗi khi nhập cân nặng không phải là số.
- **Mục đích**: Xác minh rằng phương thức `OpenSaveWeight` hiển thị thông báo lỗi nếu giá trị `WeightText` không phải là số.
- **Các bước thực hiện**:
  1. Gán `WeightText = "abc"`.
  2. Gọi phương thức `_viewModel.OpenSaveWeight()`.
- **Kết quả mong đợi**: Phương thức hiển thị thông báo lỗi "Lỗi: Cân nặng phải là một số".
- **Kết quả thực tế**: Phương thức hiển thị thông báo lỗi "Lỗi: Cân nặng phải là một số".

---

#### 5.1.2 `OpenSaveWeight_ShouldShowErrorDialog_WhenWeightIsOutOfRange`

- **ID**: TC02
- **Tên**: Kiểm tra thông báo lỗi khi cân nặng vượt ngoài miền giá trị cho phép.
- **Mục đích**: Đảm bảo rằng phương thức hiển thị lỗi khi cân nặng vượt miền giá trị.
- **Các bước thực hiện**:
  1. Gán `WeightText = "250"`.
  2. Gọi phương thức `_viewModel.OpenSaveWeight()`.
- **Kết quả mong đợi**: Phương thức hiển thị thông báo lỗi "Lỗi: Ứng dụng chỉ hỗ trợ cân nặng từ 30kg đến 200kg".
- **Kết quả thực tế**: Phương thức hiển thị thông báo lỗi "Lỗi: Ứng dụng chỉ hỗ trợ cân nặng từ 30kg đến 200kg".

---

#### 5.1.3 `OpenSaveWeight_ShouldUpdateWeight_WhenWeightIsValid`

- **ID**: TC03
- **Tên**: Kiểm tra cập nhật cân nặng hợp lệ.
- **Mục đích**: Đảm bảo rằng phương thức cập nhật cân nặng khi giá trị hợp lệ.
- **Các bước thực hiện**:
  1. Gán `WeightText = "70"` và `_customer_id = "123"`.
  2. Giả lập phương thức `Put` của `ApiServicesClient` trả về thành công.
  3. Gọi phương thức `_viewModel.OpenSaveWeight()`.
- **Kết quả mong đợi**:
  - Phương thức `Put` được gọi đúng cách với URL: `api/Customerhealth/update/123/weight/70`.
  - Cân nặng được cập nhật trong ViewModel.
- **Kết quả thực tế**:
  - Phương thức `Put` được gọi đúng cách với URL: `api/Customerhealth/update/123/weight/70`.
  - Cân nặng được cập nhật trong ViewModel.

---

#### 5.1.4 `OpenSaveWeight_ShouldShowErrorDialog_WhenApiThrowsException`

- **ID**: TC04
- **Tên**: Kiểm tra thông báo lỗi khi API gặp lỗi.
- **Mục đích**: Xác minh rằng phương thức hiển thị lỗi khi API trả về lỗi.
- **Các bước thực hiện**:
  1. Gán `WeightText = "70"` và `_customer_id = "123"`.
  2. Giả lập phương thức `Put` của `ApiServicesClient` ném ngoại lệ "API error".
  3. Gọi phương thức `_viewModel.OpenSaveWeight()`.
- **Kết quả mong đợi**: Phương thức hiển thị thông báo lỗi "Lỗi hệ thống: API error".
- **Kết quả thực tế**: Phương thức hiển thị thông báo lỗi "Lỗi hệ thống: API error".

---

#### 5.1.5 `OnNavigatedTo_CurrentDailydiaryNotNull_ProgressValueLessThanOrEqualTo100`

- **ID**: TC05
- **Tên**: Kiểm tra tiến độ calo tiêu thụ khi không vượt mức.
- **Mục đích**: Đảm bảo các giá trị tiến độ được tính toán chính xác khi dữ liệu hợp lệ.
- **Các bước thực hiện**:
  1. Thiết lập giá trị trả về cho `Dailydiary`.
  2. Gọi phương thức `_viewModel.OnNavigatedTo(null)`.
- **Kết quả mong đợi**:
  - Các thuộc tính như `WeightText`, `GoalCalories`, `RemainCalories` được thiết lập đúng.
  - `ProgressValue` nhỏ hơn hoặc bằng 100.
- **Kết quả thực tế**:
  - Các thuộc tính như `WeightText`, `GoalCalories`, `RemainCalories` được thiết lập đúng.
  - `ProgressValue` nhỏ hơn hoặc bằng 100.

---

#### 5.1.6 `OnNavigatedTo_CurrentDailydiaryNotNull_ProgressValueGreaterThan100`

- **ID**: TC06
- **Tên**: Kiểm tra tiến độ calo tiêu thụ khi vượt mức.
- **Mục đích**: Đảm bảo các giá trị tiến độ được tính toán chính xác khi calo vượt mục tiêu.
- **Các bước thực hiện**:
  1. Thiết lập giá trị trả về cho `Dailydiary` với `calories_remain = -100`.
  2. Gọi phương thức `_viewModel.OnNavigatedTo(null)`.
- **Kết quả mong đợi**:
  - `ProgressValue` lớn hơn 100.
  - Thuộc tính `IsOverGoalCalories` là `true`.
- **Kết quả thực tế**:
  - `ProgressValue` lớn hơn 100.
  - Thuộc tính `IsOverGoalCalories` là `true`.

---

#### 5.1.7 `OnNavigatedTo_CurrentDailydiaryNull_NewDailydiaryCreated`

- **ID**: TC07
- **Tên**: Kiểm tra tạo mới nhật ký dinh dưỡng khi không có dữ liệu hiện hành.
- **Mục đích**: Xác minh rằng nhật ký dinh dưỡng được tạo mới khi cần.
- **Các bước thực hiện**:
  1. Thiết lập giá trị trả về của `Dailydiary` là `null`.
  2. Gọi phương thức `_viewModel.OnNavigatedTo(null)`.
- **Kết quả mong đợi**:
  - Phương thức `Post` được gọi để tạo mới `Dailydiary`.
  - Thuộc tính `WeightText`, `GoalCalories` được cập nhật đúng.
- **Kết quả thực tế**:
  - Phương thức `Post` được gọi để tạo mới `Dailydiary`.
  - Thuộc tính `WeightText`, `GoalCalories` được cập nhật đúng.

---

#### 5.1.8 `OnNavigatedTo_ShouldShowErrorDialog_WhenApiPostFails`

- **ID**: TC08
- **Tên**: Kiểm tra thông báo lỗi khi API gặp vấn đề trong việc thêm mới nhật ký dinh dưỡng.
- **Mục đích**: Đảm bảo rằng lỗi API được xử lý đúng.
- **Các bước thực hiện**:
  1. Giả lập phương thức `Post` của `ApiServicesClient` ném ngoại lệ "API POST failed".
  2. Gọi phương thức `_viewModel.OnNavigatedTo(null)`.
- **Kết quả mong đợi**: Phương thức hiển thị thông báo lỗi "API POST failed".
- **Kết quả thực tế**: Phương thức hiển thị thông báo lỗi "API POST failed".

---

#### 5.1.9 `OnNavigatedTo_PlandetailsNotNull_CurrentDayWorkoutDetailFound`

- **ID**: TC09
- **Tên**: Kiểm tra dữ liệu ngày tập hiện tại.
- **Mục đích**: Đảm bảo thuộc tính của ngày tập được cập nhật đúng.
- **Các bước thực hiện**:
  1. Thiết lập dữ liệu kế hoạch (`Plandetail`) có chứa `Workoutdetail` cho ngày hiện tại.
  2. Gọi phương thức `_viewModel.OnNavigatedTo(null)`.
- **Kết quả mong đợi**:
  - Các thuộc tính liên quan đến ngày tập được cập nhật đúng.
- **Kết quả thực tế**:
  - Các thuộc tính liên quan đến ngày tập được cập nhật đúng.

---

#### 5.1.10 `OnNavigatedTo_PlandetailsNotNull_CurrentDayWorkoutDetailNotFound`

- **ID**: TC10
- **Tên**: Kiểm tra dữ liệu ngày nghỉ.
- **Mục đích**: Đảm bảo thuộc tính của ngày nghỉ được cập nhật đúng.
- **Các bước thực hiện**:
  1. Thiết lập dữ liệu kế hoạch (`Plandetail`) không chứa `Workoutdetail`.
  2. Gọi phương thức `_viewModel.OnNavigatedTo(null)`.
- **Kết quả mong đợi**:
  - Các thuộc tính liên quan đến ngày nghỉ được thiết lập.
- **Kết quả thực tế**:
  - Các thuộc tính liên quan đến ngày nghỉ được thiết lập.

---

#### 5.1.11 `OnNavigatedTo_PlandetailsNull`

- **ID**: TC11
- **Tên**: Kiểm tra trạng thái khi không có kế hoạch tập luyện.
- **Mục đích**: Đảm bảo trạng thái "Chưa có kế hoạch" được hiển thị khi không có dữ liệu kế hoạch.
- **Các bước thực hiện**:
  1. Thiết lập giá trị trả về của `Plandetail` là `null`.
  2. Gọi phương thức `_viewModel.OnNavigatedTo(null)`.
- **Kết quả mong đợi**:
  - Các thuộc tính hiển thị trạng thái "Chưa có kế hoạch".
- **Kết quả thực tế**:
  - Các thuộc tính hiển thị trạng thái "Chưa có kế hoạch".

---

### 5.2 Nutrition View Model

#### 5.2.1 `LoadDailyDiary_ShouldPopulateItems_WhenApiReturnsData`

- **ID**: TC12
- **Tên**: Kiểm tra việc tải danh sách thức ăn từ nhật ký dinh dưỡng (daily diary) khi API trả về dữ liệu hợp lệ.
- **Mục đích**: Xác minh rằng danh sách thức ăn được điền đúng khi API trả về dữ liệu hợp lệ.
- **Các bước thực hiện**:
  1. Thiết lập API trả về dữ liệu chứa danh sách `FoodDetail` và `total_calories = 2000`.
  2. Gọi phương thức `_viewModel.LoadDailyDiary()`.
- **Kết quả mong đợi**:
  - `BreakfastItems.Count = 1`.
  - `LunchItems.Count = 1`.
  - `DinnerItems.Count = 0`.
  - `SnackItems.Count = 0`.
  - `TotalCaloriesExpression = "2,000 - 400 = 1,600"`.
- **Kết quả thực tế**:
  - `BreakfastItems.Count = 1`.
  - `LunchItems.Count = 1`.
  - `DinnerItems.Count = 0`.
  - `SnackItems.Count = 0`.
  - `TotalCaloriesExpression = "2,000 - 400 = 1,600"`.

---

#### 5.2.2 `LoadDailyDiary_ShouldShowErrorDialog_WhenApiThrowsException`

- **ID**: TC13
- **Tên**: Kiểm tra việc hiển thị thông báo lỗi khi tải danh sách thức ăn từ nhật ký dinh dưỡng (daily diary) gặp lỗi API.
- **Mục đích**: Xác minh rằng thông báo lỗi hiển thị đúng khi API gặp lỗi.
- **Các bước thực hiện**:
  1. Thiết lập API ném lỗi `Exception("API error")`.
  2. Gọi phương thức `_viewModel.LoadDailyDiary()`.
- **Kết quả mong đợi**: Hiển thị thông báo lỗi "API error".
- **Kết quả thực tế**: Hiển thị thông báo lỗi "API error".

---

#### 5.2.3 `LoadFoodLibrary_ShouldPopulateItems_WhenApiReturnsData`

- **ID**: TC14
- **Tên**: Kiểm tra việc tải thư viện thức ăn khi API trả về dữ liệu hợp lệ.
- **Mục đích**: Đảm bảo rằng danh sách thư viện thức ăn được điền đúng khi API trả về dữ liệu hợp lệ.
- **Các bước thực hiện**:
  1. Thiết lập API trả về danh sách `Food` với hai item: "Apple" và "Banana".
  2. Gọi phương thức `_viewModel.LoadFoodLibraryAsync()`.
- **Kết quả mong đợi**:
  - `FilteredFoodLibraryItems.Count = 2`.
  - Phần tử đầu tiên trong danh sách có `food_name = "Apple"`.
- **Kết quả thực tế**:
  - `FilteredFoodLibraryItems.Count = 2`.
  - Phần tử đầu tiên trong danh sách có `food_name = "Apple"`.

---

#### 5.2.4 `AddFoodToMeal_ShouldCallApiAndUpdateData_WhenValidMealIsSelected`

- **ID**: TC15
- **Tên**: Kiểm tra việc thêm thức ăn vào bữa ăn khi API trả về dữ liệu hợp lệ.
- **Mục đích**: Xác minh rằng phương thức thêm thức ăn vào bữa ăn hoạt động đúng khi dữ liệu hợp lệ.
- **Các bước thực hiện**:
  1. Giả lập phương thức `ShowMealSelectionDialogAsync` trả về "Bữa Sáng".
  2. Thiết lập API trả về thành công khi gọi `Put` và trả về dữ liệu sau khi gọi `Get`.
  3. Gọi phương thức `_viewModel.AddFoodToMealAsync(selectedFood)`.
- **Kết quả mong đợi**:
  - Phương thức `Put` được gọi đúng cách.
  - Phương thức `Get` được gọi lại để tải lại dữ liệu.
- **Kết quả thực tế**:
  - Phương thức `Put` được gọi đúng cách.
  - Phương thức `Get` được gọi lại để tải lại dữ liệu.

---

#### 5.2.5 `AddFoodToMeal_ShouldShowErrorDialog_WhenApiPutThrowsException`

- **ID**: TC16
- **Tên**: Kiểm tra việc hiển thị thông báo lỗi khi thêm thức ăn vào bữa ăn và API gặp lỗi.
- **Mục đích**: Đảm bảo rằng thông báo lỗi hiển thị đúng khi API gặp lỗi.
- **Các bước thực hiện**:
  1. Giả lập phương thức `ShowMealSelectionDialogAsync` trả về "Bữa Sáng".
  2. Thiết lập API ném lỗi `Exception("API error during PUT")`.
  3. Gọi phương thức `_viewModel.AddFoodToMealAsync(selectedFood)`.
- **Kết quả mong đợi**: Hiển thị thông báo lỗi "API error during PUT".
- **Kết quả thực tế**: Hiển thị thông báo lỗi "API error during PUT".

---

#### 5.2.6 `DeleteFoodFromMeal_ShouldRemoveItemFromList_WhenApiCallSucceeds`

- **ID**: TC17
- **Tên**: Kiểm tra việc xóa thức ăn khỏi bữa ăn khi API thành công.
- **Mục đích**: Đảm bảo rằng thức ăn được xóa khỏi danh sách khi API thành công.
- **Các bước thực hiện**:
  1. Thêm một item vào `BreakfastItems`.
  2. Thiết lập API trả về thành công khi gọi `Delete`.
  3. Gọi phương thức `_viewModel.DeleteFoodFromMealAsync(foodDetail)`.
- **Kết quả mong đợi**: Item bị xóa khỏi danh sách `BreakfastItems`.
- **Kết quả thực tế**: Item bị xóa khỏi danh sách `BreakfastItems`.

---

#### 5.2.7 `DeleteFoodFromMeal_ShouldShowErrorDialog_WhenApiDeleteThrowsException`

- **ID**: TC18
- **Tên**: Kiểm tra việc hiển thị thông báo lỗi khi xóa thức ăn khỏi bữa ăn và API gặp lỗi.
- **Mục đích**: Xác minh rằng thông báo lỗi hiển thị đúng khi API gặp lỗi.
- **Các bước thực hiện**:
  1. Thiết lập API ném lỗi `Exception("API error during DELETE")`.
  2. Gọi phương thức `_viewModel.DeleteFoodFromMealAsync(foodDetail)`.
- **Kết quả mong đợi**: Hiển thị thông báo lỗi "API error during DELETE".
- **Kết quả thực tế**: Hiển thị thông báo lỗi "API error during DELETE".

---

### 5.3 Plan View Model

#### 5.3.1 `LoadPlanDetailData_ShouldSetCorrectProperties_WhenApiReturnsValidData`

- **ID**: TC19
- **Tên**: Kiểm tra khi tải thông tin kế hoạch thành công, các thuộc tính trong ViewModel được thiết lập chính xác.
- **Mục đích**: Đảm bảo các thuộc tính `StartDate`, `EndDate`, `PlanName` và danh sách `WorkoutDetailsItems` được thiết lập đúng khi API trả về dữ liệu hợp lệ.
- **Các bước thực hiện**:
  1. Thiết lập API trả về dữ liệu kế hoạch hợp lệ.
  2. Gọi phương thức `_viewModel.LoadPlanDetailData()`.
- **Kết quả mong đợi**:
  - `StartDate = 12/11/2024`.
  - `EndDate = 07/01/2025`.
  - `PlanName = "Kế Hoạch 4 Ngày"`.
  - `WorkoutDetailsItems.Count = 2`.
- **Kết quả thực tế**:
  - Trùng với kết quả mong đợi.

---

#### 5.3.2 `LoadPlanDetailData_ShouldHandleEmptyWorkoutDetails`

- **ID**: TC20
- **Tên**: Kiểm tra xử lý trường hợp danh sách ngày tập trong kế hoạch trống.
- **Mục đích**: Đảm bảo xử lý đúng khi danh sách ngày tập trong kế hoạch trống hoặc `null`.
- **Các bước thực hiện**:
  1. Thiết lập API trả về dữ liệu kế hoạch với danh sách `Workoutdetails = null`.
  2. Gọi phương thức `_viewModel.LoadPlanDetailData()`.
- **Kết quả mong đợi**: Hiển thị thông báo lỗi "Lỗi hệ thống".
- **Kết quả thực tế**: Hiển thị thông báo lỗi "Lỗi hệ thống".

---

#### 5.3.3 `LoadPlanDetailData_ShouldShowErrorDialog_WhenApiThrowsException`

- **ID**: TC21
- **Tên**: Kiểm tra khi load thông tin kế hoạch thất bại do lỗi API, đảm bảo thông báo lỗi hiển thị đúng.
- **Mục đích**: Đảm bảo thông báo lỗi hiển thị khi API gặp lỗi.
- **Các bước thực hiện**:
  1. Thiết lập API ném lỗi `Exception("API Error")`.
  2. Gọi phương thức `_viewModel.LoadPlanDetailData()`.
- **Kết quả mong đợi**: Hiển thị thông báo lỗi "API Error".
- **Kết quả thực tế**: Hiển thị thông báo lỗi "API Error".

---

#### 5.3.4 `LoadCurrentWeekDays_ShouldSetCorrectWeekNumberAndDaysHaveCompletedExercise`

- **ID**: TC22
- **Tên**: Kiểm tra logic tính toán thông tin ngày tập khi ngày hiện tại có bài tập và đã hoàn thành.
- **Mục đích**: Đảm bảo trạng thái ngày tập và tuần hiện tại được tính toán đúng khi bài tập đã hoàn thành.
- **Các bước thực hiện**:
  1. Thiết lập ngày hiện tại là 15/11/2024.
  2. Thiết lập kế hoạch bắt đầu từ 12/11/2024.
  3. Gọi phương thức `_viewModel.LoadCurrentWeekDays(startDate)`.
- **Kết quả mong đợi**:
  - `WeekNumber = 1`.
  - `Day4 = "15/11"`.
  - `StatusText = "Bạn đã hoàn thành bài tập ngày hôm nay (15/11)"`.
  - `StartExerciseText = "Tập lại"`.
  - `WorkoutButtonVisibility = Visible`.
- **Kết quả thực tế**: Trùng với kết quả mong đợi.

---

#### 5.3.5 `LoadCurrentWeekDays_ShouldSetCorrectWeekNumberAndDaysHaveUncompletedExercise`

- **ID**: TC23
- **Tên**: Kiểm tra logic tính toán thông tin ngày tập khi ngày hiện tại có bài tập nhưng chưa hoàn thành.
- **Mục đích**: Đảm bảo trạng thái ngày tập và tuần hiện tại được tính toán đúng khi bài tập chưa hoàn thành.
- **Các bước thực hiện**:
  1. Thiết lập ngày hiện tại là 23/11/2024.
  2. Thiết lập kế hoạch bắt đầu từ 12/11/2024.
  3. Gọi phương thức `_viewModel.LoadCurrentWeekDays(startDate)`.
- **Kết quả mong đợi**:
  - `WeekNumber = 2`.
  - `Day5 = "23/11"`.
  - `StatusText = "Bạn vẫn chưa hoàn thành bài tập hôm nay (23/11)"`.
  - `StartExerciseText = "Bắt đầu bài tập"`.
  - `WorkoutButtonVisibility = Visible`.
- **Kết quả thực tế**: Trùng với kết quả mong đợi.

---

#### 5.3.6 `LoadCurrentWeekDays_ShouldSetCorrectWeekNumberAndDaysHaveNoExercise`

- **ID**: TC24
- **Tên**: Kiểm tra logic tính toán thông tin ngày tập, trường hợp ngày không có bài tập (ngày nghỉ).
- **Mục đích**: Đảm bảo trạng thái ngày tập và tuần hiện tại được tính toán đúng khi không có bài tập.
- **Các bước thực hiện**:
  1. Thiết lập ngày hiện tại là 01/12/2024.
  2. Thiết lập kế hoạch bắt đầu từ 12/11/2024.
  3. Gọi phương thức `_viewModel.LoadCurrentWeekDays(startDate)`.
- **Kết quả mong đợi**:
  - `WeekNumber = 3`.
  - `Day6 = "01/12"`.
  - `StatusText = "Hôm nay là ngày nghỉ"`.
  - `WorkoutButtonVisibility = Collapsed`.
- **Kết quả thực tế**: Trùng với kết quả mong đợi.

---

#### 5.3.7 `LoadCurrentWeekDays_ShouldHandleEmptyWorkoutDetails`

- **ID**: TC25
- **Tên**: Kiểm tra trường hợp dữ liệu ngày tập bị trống (do trigger lỗi), đảm bảo tuần vẫn được khởi tạo.
- **Mục đích**: Đảm bảo rằng tuần vẫn được khởi tạo đúng khi không có dữ liệu bài tập.
- **Các bước thực hiện**:
  1. Thiết lập ngày hiện tại là 01/12/2024.
  2. Thiết lập danh sách `WorkoutDetailsItems` trống.
  3. Gọi phương thức `_viewModel.LoadCurrentWeekDays(startDate)`.
- **Kết quả mong đợi**:
  - `WeekDaysItems` được khởi tạo với 7 ngày.
- **Kết quả thực tế**: Trùng với kết quả mong đợi.

---

#### 5.3.8 `PlayingWorkoutExercises_ShouldCallApiAndUpdateStatus_WhenExerciseIsFinished`

- **ID**: TC26
- **Tên**: Kiểm tra logic người dùng chọn thực hiện bài tập, xem hết và hoàn thành bài tập.
- **Mục đích**: Đảm bảo trạng thái bài tập được cập nhật khi bài tập hoàn thành.
- **Các bước thực hiện**:
  1. Thiết lập ngày hiện tại là 23/11/2024.
  2. Giả lập người dùng hoàn thành bài tập.
  3. Gọi phương thức `_viewModel.PlayingWorkoutExercises()`.
- **Kết quả mong đợi**:
  - `StatusText = "Bạn đã hoàn thành bài tập ngày hôm nay (23/11)"`.
  - `StartExerciseText = "Tập lại"`.
  - Phương thức `Put` được gọi đúng cách.
- **Kết quả thực tế**: Trùng với kết quả mong đợi.

---

#### 5.3.9 `PlayingWorkoutExercises_ShouldNotCallApi_WhenExerciseIsNotFinished`

- **ID**: TC27
- **Tên**: Kiểm tra trường hợp người dùng chọn thực hiện bài tập, nhưng hủy giữa chừng.
- **Mục đích**: Đảm bảo API không được gọi khi bài tập bị hủy giữa chừng.
- **Các bước thực hiện**:
  1. Giả lập người dùng hủy bài tập.
  2. Gọi phương thức `_viewModel.PlayingWorkoutExercises()`.
- **Kết quả mong đợi**:
  - API không được gọi.
  - `StatusText` và `StartExerciseText` không thay đổi.
- **Kết quả thực tế**: Trùng với kết quả mong đợi.

---

#### 5.3.10 `PlayingWorkoutExercises_ShouldShowErrorDialog_WhenApiPutFails`

- **ID**: TC28
- **Tên**: Kiểm tra trường hợp người dùng hoàn thành bài tập nhưng gọi API cập nhật thất bại.
- **Mục đích**: Đảm bảo thông báo lỗi hiển thị đúng khi API cập nhật thất bại.
- **Các bước thực hiện**:
  1. Giả lập API ném lỗi khi gọi `Put`.
  2. Gọi phương thức `_viewModel.PlayingWorkoutExercises()`.
- **Kết quả mong đợi**:
  - Hiển thị thông báo lỗi "không thể cập nhật trạng thái bài tập".
- **Kết quả thực tế**: Trùng với kết quả mong đợi.

---

### 5.4 Diary ViewModel

#### 5.4.1 `LoadDailyDiary_WithValidData_ShouldPopulateCollections`

- **ID**: TC29
- **Tên**: Kiểm tra xem phương thức `LoadDailyDiary` có điền đúng thông tin vào các bộ sưu tập khi nhận dữ liệu hợp lệ.
- **Mục đích**: Xác minh rằng các thuộc tính của ViewModel được thiết lập chính xác khi API trả về dữ liệu nhật ký dinh dưỡng hợp lệ.
- **Các bước thực hiện**:
  1. Giả lập dữ liệu `Dailydiary` hợp lệ với các thông tin về bữa ăn, cân nặng và calo.
  2. Gọi phương thức `_viewModel.LoadDailyDiary(date)`.
- **Kết quả mong đợi**:
  - `BreakfastItems.Count = 1`.
  - `LunchItems.Count = 1`.
  - `DinnerItems.Count = 1`.
  - `SnackItems.Count = 1`.
  - `WeightText = 70`.
  - `BurnedCalories = 1500`.
  - `TotalCalories = 2000`.
- **Kết quả thực tế**: Trùng với kết quả mong đợi.

---

#### 5.4.2 `LoadFullData_ShouldShowErrorDialog_WhenLoadDailyDiaryFails`

- **ID**: TC30
- **Tên**: Kiểm tra hiển thị thông báo lỗi khi gọi API trong phương thức `LoadFullData` thất bại.
- **Mục đích**: Xác minh rằng thông báo lỗi được hiển thị khi xảy ra lỗi trong quá trình gọi API.
- **Các bước thực hiện**:
  1. Giả lập API ném lỗi `Exception("Daily diary API error")`.
  2. Gọi phương thức `_viewModel.LoadFullData(daySelected)`.
- **Kết quả mong đợi**: Hiển thị thông báo lỗi "Daily diary API error".
- **Kết quả thực tế**: Trùng với kết quả mong đợi.

---

#### 5.4.3 `LoadWorkoudetails_WithValidPlanDetails_ShouldSetWorkoutDayProperties`

- **ID**: TC31
- **Tên**: Kiểm tra xem phương thức `LoadWorkoudetails` có thiết lập đúng các thuộc tính khi API trả về kế hoạch tập luyện hợp lệ.
- **Mục đích**: Đảm bảo rằng các thuộc tính liên quan đến ngày tập luyện được thiết lập chính xác.
- **Các bước thực hiện**:
  1. Giả lập API trả về dữ liệu `Plandetail` hợp lệ.
  2. Gọi phương thức `_viewModel.LoadWorkoudetails(date)`.
- **Kết quả mong đợi**:
  - `PlanNameText = "Test Plan"`.
  - `TypeWorkoutText = "Test Workout"`.
  - `_exerciseStatus = 2`.
- **Kết quả thực tế**: Trùng với kết quả mong đợi.

---

#### 5.4.4 `LoadWorkoudetails_NoPlanDetail_ShouldSetDefaultProperties`

- **ID**: TC32
- **Tên**: Kiểm tra xem các thuộc tính mặc định có được thiết lập đúng khi không có kế hoạch tập luyện.
- **Mục đích**: Đảm bảo rằng các thuộc tính được thiết lập mặc định khi API trả về `null`.
- **Các bước thực hiện**:
  1. Giả lập API trả về `null` cho `Plandetail`.
  2. Gọi phương thức `_viewModel.LoadWorkoudetails(date)`.
- **Kết quả mong đợi**:
  - `PlanNameText = "Chưa có kế hoạch"`.
  - `TypeWorkoutText = "Chưa có ngày tập"`.
  - `_exerciseStatus = 0`.
- **Kết quả thực tế**: Trùng với kết quả mong đợi.

---

#### 5.4.5 `LoadWorkoudetails_RestDay_ShouldSetRestDayProperties`

- **ID**: TC33
- **Tên**: Kiểm tra xem các thuộc tính có được thiết lập đúng khi ngày hiện tại là ngày nghỉ.
- **Mục đích**: Đảm bảo rằng các thuộc tính phản ánh đúng trạng thái ngày nghỉ.
- **Các bước thực hiện**:
  1. Giả lập API trả về dữ liệu `Plandetail` hợp lệ với ngày hiện tại là ngày nghỉ.
  2. Gọi phương thức `_viewModel.LoadWorkoudetails(date)`.
- **Kết quả mong đợi**:
  - `PlanNameText = "Test Plan"`.
  - `TypeWorkoutText = "Ngày nghỉ"`.
  - `_exerciseStatus = 1`.
- **Kết quả thực tế**: Trùng với kết quả mong đợi.

---

## Báo cáo kết quả kiểm thử

| **ID** | **Tên Test Case**                                                            | **Kết quả** |
| ------ | ---------------------------------------------------------------------------- | ----------- |
| TC01   | OpenSaveWeight_ShouldShowErrorDialog_WhenWeightIsNotNumeric                  | Passed      |
| TC02   | OpenSaveWeight_ShouldShowErrorDialog_WhenWeightIsOutOfRange                  | Passed      |
| TC03   | OpenSaveWeight_ShouldUpdateWeight_WhenWeightIsValid                          | Passed      |
| TC04   | OpenSaveWeight_ShouldShowErrorDialog_WhenApiThrowsException                  | Passed      |
| TC05   | OnNavigatedTo_CurrentDailydiaryNotNull_ProgressValueLessThanOrEqualTo100     | Passed      |
| TC06   | OnNavigatedTo_CurrentDailydiaryNotNull_ProgressValueGreaterThan100           | Passed      |
| TC07   | OnNavigatedTo_CurrentDailydiaryNull_NewDailydiaryCreated                     | Passed      |
| TC08   | OnNavigatedTo_ShouldShowErrorDialog_WhenApiPostFails                         | Passed      |
| TC09   | OnNavigatedTo_PlandetailsNotNull_CurrentDayWorkoutDetailFound                | Passed      |
| TC10   | OnNavigatedTo_PlandetailsNotNull_CurrentDayWorkoutDetailNotFound             | Passed      |
| TC11   | OnNavigatedTo_PlandetailsNull                                                | Passed      |
| TC12   | LoadDailyDiary_ShouldPopulateItems_WhenApiReturnsData                        | Passed      |
| TC13   | LoadDailyDiary_ShouldShowErrorDialog_WhenApiThrowsException                  | Passed      |
| TC14   | LoadFoodLibrary_ShouldPopulateItems_WhenApiReturnsData                       | Passed      |
| TC15   | AddFoodToMeal_ShouldCallApiAndUpdateData_WhenValidMealIsSelected             | Passed      |
| TC16   | AddFoodToMeal_ShouldShowErrorDialog_WhenApiPutThrowsException                | Passed      |
| TC17   | DeleteFoodFromMeal_ShouldRemoveItemFromList_WhenApiCallSucceeds              | Passed      |
| TC18   | DeleteFoodFromMeal_ShouldShowErrorDialog_WhenApiDeleteThrowsException        | Passed      |
| TC19   | LoadPlanDetailData_ShouldSetCorrectProperties_WhenApiReturnsValidData        | Passed      |
| TC20   | LoadPlanDetailData_ShouldHandleEmptyWorkoutDetails                           | Passed      |
| TC21   | LoadPlanDetailData_ShouldShowErrorDialog_WhenApiThrowsException              | Passed      |
| TC22   | LoadCurrentWeekDays_ShouldSetCorrectWeekNumberAndDaysHaveCompletedExercise   | Passed      |
| TC23   | LoadCurrentWeekDays_ShouldSetCorrectWeekNumberAndDaysHaveUncompletedExercise | Passed      |
| TC24   | LoadCurrentWeekDays_ShouldSetCorrectWeekNumberAndDaysHaveNoExercise          | Passed      |
| TC25   | LoadCurrentWeekDays_ShouldHandleEmptyWorkoutDetails                          | Passed      |
| TC26   | PlayingWorkoutExercises_ShouldCallApiAndUpdateStatus_WhenExerciseIsFinished  | Passed      |
| TC27   | PlayingWorkoutExercises_ShouldNotCallApi_WhenExerciseIsNotFinished           | Passed      |
| TC28   | PlayingWorkoutExercises_ShouldShowErrorDialog_WhenApiPutFails                | Passed      |
| TC29   | LoadDailyDiary_WithValidData_ShouldPopulateCollections                       | Passed      |
| TC30   | LoadFullData_ShouldShowErrorDialog_WhenLoadDailyDiaryFails                   | Passed      |
| TC31   | LoadWorkoudetails_WithValidPlanDetails_ShouldSetWorkoutDayProperties         | Passed      |
| TC32   | LoadWorkoudetails_NoPlanDetail_ShouldSetDefaultProperties                    | Passed      |
| TC33   | LoadWorkoudetails_RestDay_ShouldSetRestDayProperties                         | Passed      |

- Tổng số kiểm thử: 33 test case.
- Kết quả:
  - Passed: 33 test case.
  - Failed: 0 test case.
  - Skipped: 0 test case.
