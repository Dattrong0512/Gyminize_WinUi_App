using Gyminize.Core.Contracts.Services;
using Gyminize.Core.Models;

namespace Gyminize.Core.Services;
public class SampleDataService : ISampleDataService
{
    private List<Influencer> _allInfluencers;

    public SampleDataService()
    {
    }
    private static IEnumerable<Influencer> AllInfluencers()
    {
        return new List<Influencer>()
        {
            new() {
                influencer_id = 1,
                influencer_name = "Athlean-X",
                influencer_country = "Hoa Kỳ",
                influencer_platform = "Youtube",
                influencer_background = "Athlean-X là một kênh YouTube nổi tiếng trong lĩnh vực thể hình và sức khỏe, do Jeff Cavaliere, một huấn luyện viên thể hình và chuyên gia vật lý trị liệu, sáng lập. Kênh chuyên cung cấp các bài tập thể dục, hướng dẫn dinh dưỡng, và chiến lược huấn luyện để giúp người xem xây dựng cơ bắp, cải thiện sức khỏe và thể lực",
                influencer_link = "https://www.youtube.com/@athleanx"
            },

            new() {
                influencer_id = 2,
                influencer_name = "Chloe Ting",
                influencer_country = "Úc",
                influencer_platform = "Youtube",
                influencer_background = "Chloe Ting là một huấn luyện viên thể hình nổi tiếng, người Úc gốc Malaysia. Cô được biết đến rộng rãi nhờ các chương trình tập luyện miễn phí trên YouTube, đặc biệt là các video hướng dẫn giảm cân và tăng cơ. Chloe Ting đã thu hút hàng triệu người theo dõi trên toàn thế giới nhờ vào các bài tập hiệu quả, dễ thực hiện tại nhà và cam kết giúp mọi người cải thiện sức khỏe và thể hình mà không cần dụng cụ phức tạp",
                influencer_link = "https://www.youtube.com/c/ChloeTing"
            },

            new() {
                influencer_id = 3,
                influencer_name = "Chris Heria",
                influencer_country = "Hoa Kỳ",
                influencer_platform = "Youtube",
                influencer_background = "Chris Heria là một huấn luyện viên thể hình nổi tiếng và là người sáng lập THENX, một chương trình tập luyện thể dục thể thao chuyên về calisthenics. Kênh YouTube của anh chủ yếu tập trung vào việc chia sẻ các bài tập thể hình, đặc biệt là các bài tập không cần dụng cụ (bodyweight training), nhằm giúp người xem phát triển sức mạnh, sự linh hoạt và thể lực tổng thể",
                influencer_link = "https://www.youtube.com/channel/UCaBqRxHEMomgFU-AkSfodCw"
            },

            new() {
                influencer_id = 4,
                influencer_name = "Viktor Axelsen",
                influencer_country = "Đan Mạch",
                influencer_platform = "Youtube",
                influencer_background = "Kênh YouTube của Viktor Axelsen chủ yếu chia sẻ về kỹ thuật cầu lông, chế độ luyện tập, phục hồi và dinh dưỡng để duy trì sức khỏe và thể lực. Anh cung cấp các bài tập nâng cao, chiến thuật thi đấu, cùng những kinh nghiệm về việc giữ thăng bằng, tập trung và vượt qua chấn thương. Ngoài ra, Viktor còn chia sẻ về chế độ ăn uống và thói quen sinh hoạt giúp tối ưu hóa hiệu suất thể thao. Kênh của anh không chỉ hữu ích cho người chơi cầu lông mà còn cho bất kỳ ai muốn cải thiện sức khỏe và thể lực.",
                influencer_link = "https://www.youtube.com/@viktoraxelsen7830"
            },

            new() {
                influencer_id = 5,
                influencer_name = "Popsugar Fitness",
                influencer_country = "Hoa Kỳ",
                influencer_platform = "Youtube",
                influencer_background = "PS- Fit là kênh YouTube của Popsugar Fitness, chuyên cung cấp các video tập luyện thể dục, thể hình, và các bài hướng dẫn về lối sống lành mạnh. Kênh này được biết đến với các chương trình tập luyện đa dạng, phù hợp với nhiều cấp độ thể lực và nhu cầu khác nhau, từ yoga, pilates đến các bài tập cardio, tập sức mạnh và các bài tập thể dục cho sức khỏe tâm thần.",
                influencer_link = "https://www.youtube.com/@PS_Fit"
            },

            new() {
                influencer_id = 6,
                influencer_name = "Fitness Blender",
                influencer_country = "Hoa Kỳ",
                influencer_platform = "Youtube",
                influencer_background = "Fitness Blender là một kênh YouTube nổi tiếng với các bài tập thể dục trực tuyến. Kênh này được sáng lập bởi Daniel và Kelli Segars, và cung cấp các chương trình tập luyện miễn phí, bao gồm các bài tập thể dục toàn thân, giảm cân, sức bền, và thể lực",
                influencer_link = "https://www.youtube.com/@fitnessblender"
            },


            new() {
                influencer_id = 7,
                influencer_name = "Yoga with Adriene",
                influencer_country = "Hoa Kỳ",
                influencer_platform = "Youtube",
                influencer_background = "Kênh Yoga With Adriene trên YouTube, được dẫn dắt bởi Adriene Mishler, là một trong những kênh yoga phổ biến và có ảnh hưởng nhất trên nền tảng này. Adriene, một huấn luyện viên yoga nổi tiếng, đã tạo ra một cộng đồng người hâm mộ lớn và gắn bó thông qua các video hướng dẫn yoga dễ tiếp cận và mang tính thư giãn cao",
                influencer_link = "https://www.youtube.com/@yogawithadriene"
            },

            new() {
                influencer_id = 8,
                influencer_name = "Guru Mann",
                influencer_country = "Ấn Độ",
                influencer_platform = "Youtube",
                influencer_background = "Kênh Guru Mann Fitness trên YouTube chuyên cung cấp các nội dung liên quan đến thể hình, sức khỏe và dinh dưỡng. Được sáng lập bởi Guru Mann, một chuyên gia về thể hình và dinh dưỡng, kênh này mang đến những video chia sẻ kiến thức về cách tập luyện, chế độ ăn uống, các bài tập thể dục hiệu quả và các mẹo để duy trì sức khỏe tốt. Ngoài ra, Guru Mann cũng chia sẻ những chương trình tập luyện cho những người mới bắt đầu, các video hướng dẫn về dinh dưỡng hợp lý cho cơ thể và cách cải thiện vóc dáng một cách khoa học",
                influencer_link = "https://www.youtube.com/@GuruMannFitness"
            },

            new() {
                influencer_id = 9,
                influencer_name = "Joana Soh",
                influencer_country = "Malaysia",
                influencer_platform = "Youtube",
                influencer_background = "Joana Soh là một chuyên gia về fitness và dinh dưỡng, nổi tiếng với những video hướng dẫn về thể dục, chế độ ăn uống lành mạnh, và lối sống cân đối. Cô là một huấn luyện viên cá nhân (personal trainer) và đã chia sẻ nhiều lời khuyên qua các video trên YouTube và các nền tảng xã hội khác. Cô đặc biệt tập trung vào các bài tập thể dục đơn giản nhưng hiệu quả, phù hợp cho mọi đối tượng, đặc biệt là những người muốn duy trì một lối sống khỏe mạnh nhưng không có quá nhiều thời gian",
                influencer_link = "https://www.youtube.com/user/joannasohofficial"
            },

            new() {
                influencer_id = 10,
                influencer_name = "Pick Up Limes",
                influencer_country = "Canada",
                influencer_platform = "Youtube",
                influencer_background = "Kênh YouTube Pick Up Limes do Sadia Badiei sáng lập và điều hành, chuyên chia sẻ các công thức nấu ăn, mẹo ăn uống lành mạnh, và các lời khuyên về dinh dưỡng. Kênh này tập trung vào việc cung cấp những bữa ăn lành mạnh, dễ làm và giúp người xem duy trì lối sống cân bằng và khỏe mạnh. Pick Up Limes nổi bật vì những video đầy cảm hứng và hướng dẫn rõ ràng, sử dụng các nguyên liệu tự nhiên và dễ kiếm",
                influencer_link = "https://www.youtube.com/@PickUpLimes"
            },

            new() {
                influencer_id = 11,
                influencer_name = "Văn Tới Calisthenics",
                influencer_country = "Việt Nam",
                influencer_platform = "Youtube",
                influencer_background = "Văn Tới là một YouTuber nổi bật trong cộng đồng calisthenics (tập thể dục với trọng lượng cơ thể) tại Việt Nam. Anh chia sẻ các video hướng dẫn luyện tập, chiến lược tập luyện calisthenics và các bài tập cơ bản như hít đất, chống đẩy, kéo xà, với mục tiêu giúp người xem cải thiện sức mạnh và sự linh hoạt. Nội dung của Văn Tới không chỉ tập trung vào việc phát triển thể lực mà còn chia sẻ về chế độ ăn uống, dinh dưỡng và lối sống lành mạnh. Anh tạo ra một cộng đồng yêu thể thao, giúp mọi người tham gia vào các bài tập calisthenics để có một cơ thể khỏe mạnh hơn",
                influencer_link = "https://www.youtube.com/@vantoicalisthenics-style4607"
            },

            new() {
                influencer_id = 12,
                influencer_name = "Nguyễn Bảo Bằng",
                influencer_country = "Việt Nam",
                influencer_platform = "Youtube",
                influencer_background = "Kênh YouTube của Nguyễn Bảo Bằng chuyên chia sẻ các bài tập gym hiệu quả và kỹ thuật tập luyện khoa học, giúp bạn cải thiện thể lực, tăng cường sức khỏe và đạt được mục tiêu thể hình. Các video trên kênh tập trung vào nhiều khía cạnh như tăng cân, giảm cân, phát triển cơ bắp, và cải thiện chiều cao. Ngoài ra, Bằng cũng cung cấp những kiến thức bổ ích về dinh dưỡng để hỗ trợ quá trình tập luyện và đạt được kết quả tối ưu. Kênh là nguồn tài liệu tuyệt vời cho những ai muốn xây dựng một lối sống lành mạnh và cải thiện vóc dáng một cách bền vững",
                influencer_link = "https://www.youtube.com/c/NguyenBaoBang"
            },

            new() {
                influencer_id = 13,
                influencer_name = "Phan Nữ Uyên Nhi",
                influencer_country = "Việt Nam",
                influencer_platform = "Youtube",
                influencer_background = "Phan Nữ Uyên Nhi là một huấn luyện viên Yoga và Pilates chuyên nghiệp, với hơn 7 năm kinh nghiệm trong lĩnh vực fitness. Cô đặc biệt chuyên sâu vào các chương trình luyện tập dành cho phụ nữ, bao gồm các lớp Yoga và Pilates giúp cải thiện sức khỏe, sự dẻo dai và phát triển thể hình",
                influencer_link = "https://www.youtube.com/c/PhanNuUyenNhi"
            },

            new() {
                influencer_id = 14,
                influencer_name = "SHINPHAMM",
                influencer_country = "Việt Nam",
                influencer_platform = "Youtube",
                influencer_background = "SHINPHAMM là một YouTuber nổi tiếng tại Việt Nam, chuyên chia sẻ các nội dung liên quan đến thể hình, gym và lifestyle. Kênh của anh thu hút sự chú ý nhờ vào các video hướng dẫn tập luyện, chia sẻ chế độ dinh dưỡng, cách xây dựng cơ bắp, giảm mỡ, và cải thiện sức khỏe",
                influencer_link = "https://www.youtube.com/@SHINPHAMM"
            },


            new() {
                influencer_id = 15,
                influencer_name = "PHAN BẢO LONG",
                influencer_country = "Việt Nam",
                influencer_platform = "Youtube",
                influencer_background = "Phan Bảo Long, cựu giảng viên Khoa Y Dược, Đại học Tây Nguyên, là một chuyên gia về giảm cân và tăng cân theo phương pháp khoa học. Anh không chỉ là một huấn luyện viên thể hình dày dặn kinh nghiệm, mà còn là người truyền cảm hứng cho nhiều người về lối sống lành mạnh. Minh chứng cho trình độ chuyên môn của mình, Phan Bảo Long đã giúp Sơn Tùng, người từng nặng 168kg, mắc bệnh béo phì, rối loạn chuyển hóa, giảm cân an toàn và hiệu quả xuống còn 90kg. Với những bài tập được thiết kế riêng cùng chế độ ăn uống khoa học chuẩn y khoa, Phan Bảo Long đang lan tỏa thông điệp về sức khỏe và sắc đẹp bền vững đến cộng đồng",
                influencer_link = "https://www.youtube.com/@phanbaolong1019"
            },


            new() {
                influencer_id = 16,
                influencer_name = "SmallGym",
                influencer_country = "Việt Nam",
                influencer_platform = "Youtube",
                influencer_background = "SmallGym là một kênh YouTube nổi bật trong cộng đồng yêu thích thể hình tại Việt Nam. Kênh này được điều hành bởi những người đam mê thể dục thể thao và fitness, chuyên cung cấp các video liên quan đến các chủ đề tập luyện, xây dựng cơ bắp, dinh dưỡng và lối sống lành mạnh. SmallGym tập trung vào việc hỗ trợ người xem có thể tập luyện hiệu quả tại các phòng gym nhỏ, tại nhà hoặc những không gian hạn chế",
                influencer_link = "https://www.youtube.com/@SmallGym"
            },

            new() {
                influencer_id = 17,
                influencer_name = "An Nguyễn Fitness",
                influencer_country = "Việt Nam",
                influencer_platform = "Youtube",
                influencer_background = "An Nguyen Fitness là một YouTuber và influencer nổi tiếng trong cộng đồng thể hình tại Việt Nam. Anh được biết đến với các video chia sẻ về những chủ đề liên quan đến thể hình, chế độ dinh dưỡng và phong cách sống lành mạnh. An Nguyễn, người sáng lập kênh An Nguyen Fitness, đã tạo dựng được một cộng đồng lớn và được đông đảo người yêu thích nhờ vào sự chia sẻ kiến thức bổ ích và kinh nghiệm thực tế của mình trong việc tập luyện và cải thiện sức khỏe",
                influencer_link = "https://www.youtube.com/@shredan72"
            },

            new() {
                influencer_id = 18,
                influencer_name = "Coach Vincent Fitness",
                influencer_country = "Việt Nam",
                influencer_platform = "Youtube",
                influencer_background = "Coach Vincent Fitness là một YouTuber nổi bật trong cộng đồng thể hình, đặc biệt là tại Việt Nam, và được biết đến nhiều với những video chia sẻ kiến thức về tập luyện thể thao, dinh dưỡng, và lối sống khỏe mạnh. Anh là một huấn luyện viên cá nhân (personal trainer) và có nhiều kinh nghiệm trong việc hướng dẫn mọi người đạt được mục tiêu thể hình và cải thiện sức khỏe.",
                influencer_link = "https://www.youtube.com/@CoachVincentFitness"
            },

            new() {
                influencer_id = 19,
                influencer_name = "SVFit - Gym & Cuộc sống tại nhật",
                influencer_country = "Việt Nam",
                influencer_platform = "Youtube",
                influencer_background = "SVFit là một kênh YouTube nổi bật, chủ yếu tập trung vào các chủ đề liên quan đến thể hình (gym) và cuộc sống tại Nhật Bản. Kênh này được điều hành bởi một người Việt Nam, và những video của anh ấy chủ yếu chia sẻ các trải nghiệm cá nhân về cuộc sống tại Nhật, cách tập luyện gym, chế độ ăn uống và phát triển cơ thể, cũng như những chia sẻ về lối sống khỏe mạnh",
                influencer_link = "https://www.youtube.com/@svfit-gymcuocsongtainhat5704"
            },

            new() {
                influencer_id = 20,
                influencer_name = "Phạm Kim Nhân",
                influencer_country = "Việt Nam",
                influencer_platform = "Youtube",
                influencer_background = "Phạm Kim Nhân là một YouTuber và huấn luyện viên fitness nổi tiếng tại Việt Nam, chuyên chia sẻ các video về thể hình, dinh dưỡng và lối sống lành mạnh. Với kinh nghiệm nhiều năm trong lĩnh vực luyện tập và cải thiện sức khỏe, Phạm Kim Nhân đã thu hút một lượng lớn người theo dõi nhờ vào các bài tập đơn giản, hiệu quả và những lời khuyên hữu ích cho những ai muốn cải thiện vóc dáng và sức khỏe",
                influencer_link = "https://www.youtube.com/@phamkimnhan138"
            },
        };
    }

    public async Task<IEnumerable<Influencer>> GetInfluencerListDetailsDataAsync()
    {
        _allInfluencers ??= new List<Influencer>(AllInfluencers());

        await Task.CompletedTask;
        return _allInfluencers;
    }

    
}
