using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace pong
{
    public partial class Form1 : Form
    {
        // Các biến
        // Các biến
        int xspeed;             // Tốc độ di chuyển theo trục X của quả bóng
        int yspeed;             // Tốc độ di chuyển theo trục Y của quả bóng
        int lastx;               // Vị trí X cuối cùng của chuột
        int lastx_cpu;           // Vị trí X cuối cùng của thanh điều khiển bởi máy
        int score_player;        // Điểm của người chơi
        int score_cpu;           // Điểm của máy
        int topBounds;           // Biên trên của màn hình

        int bottomBounds;        // Biên dưới của màn hình
        int leftBounds;          // Biên trái của màn hình
        int rightBounds;         // Biên phải của màn hình
        bool paused = false;     // Trạng thái tạm dừng game

        int currentState;
        int previousState;
        Dictionary<int, Dictionary<int, double>> QTable = new Dictionary<int, Dictionary<int, double>>();
        double learningRate = 0.1;
        double discountFactor = 0.9;
        Random random = new Random();
        public Form1()
        {
            InitializeComponent();

            // Cài đặt tốc độ ban đầu
            SetInitialSpeed("Dễ"); // Đặt mức độ khó mặc định là "Dễ"

            // Không cho phép click vào nút trong khi game chưa bắt đầu
            ball.Enabled = false;
            paddle.Enabled = false;

            // Lưu vị trí cuối cùng của chuột
            lastx = MousePosition.X;
            lastx_cpu = paddle2.Location.X;

            // Điểm số
            score_player = 0;
            score_cpu = 0;

            // Biên của màn hình
            topBounds = 0;
            bottomBounds = this.Height;
            leftBounds = 0;
            rightBounds = this.Width;
           

            // Ẩn văn bản tạm dừng (mặc định là hiển thị)
            pause_txt.Visible = false;

            // Double Buffer (tránh hiện tượng nhấp nháy trên màn hình)
            this.SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);

            currentState = 0;
            previousState = 0;
        }

        // Di chuyển quả bóng trong mỗi frame
        private void moveBall(object sender, EventArgs e)
        {
            // Cập nhật biên (khá lộn xộn nhưng hữu ích cho việc kiểm thử)
            topBounds = 0;
            bottomBounds = this.Height - 23;
            leftBounds = 0;
            rightBounds = this.Width;

            // Nếu không tạm dừng, di chuyển vị trí của các đối tượng
            if (!paused)
            {
                // Người chơi
                paddle.Location = new Point((int)(MousePosition.X - paddle.Width), paddle.Location.Y);
                ball.Location = new Point(ball.Location.X + xspeed, ball.Location.Y + yspeed);
                Console.WriteLine("Value of xspeed: " + xspeed);
                // Máy
                if (ball.Location.X > paddle2.Location.X)
                {
                    paddle2.Location = new Point(paddle2.Location.X + 7, paddle2.Location.Y);
                }
                else
                {
                    paddle2.Location = new Point(paddle2.Location.X - 7, paddle2.Location.Y);
                }

                // Kiểm soát quả bóng: Chạm biên trái
                if (ball.Location.X < leftBounds)
                {
                    xspeed *= -1;
                    while (ball.Location.X - 1 < leftBounds)
                    {
                        ball.Location = new Point(ball.Location.X + 1, ball.Location.Y);
                    }
                }

                // Kiểm soát quả bóng: Chạm biên phải
                if (ball.Location.X + ball.Width > rightBounds)
                {
                    xspeed *= -1;
                    while (ball.Location.X + 1 > rightBounds)
                    {
                        ball.Location = new Point(ball.Location.X - 1, ball.Location.Y);
                    }
                }

                // Kiểm soát quả bóng: Va chạm với thanh người chơi
                if (ball.Location.Y + ball.Height > paddle.Location.Y && ball.Location.X > (int)(paddle.Location.X - ball.Width / 2) && ball.Location.X + ball.Width < (int)(paddle.Location.X + paddle.Width + ball.Width / 2) && ball.Location.Y < (int)(paddle.Location.Y + paddle.Height / 2))
                {
                    yspeed *= -1;
                    xspeed = (ball.Location.X + ball.Width / 2) - (paddle.Location.X + paddle.Width / 2);
                    if (xspeed > 4)
                    {
                        xspeed = 4;
                    }
                    else if (xspeed < -4)
                    {
                        xspeed = -4;
                    }
                    else if (xspeed == 0)
                    {
                        Random a = new Random();
                        if (a.NextDouble() > .5)
                        {
                            xspeed = 2;
                        }
                        else
                        {
                            xspeed = -2;
                        }
                    }
                    while (ball.Location.Y + 1 + ball.Height > paddle.Location.Y)
                    {
                        ball.Location = new Point(ball.Location.X, ball.Location.Y - 1);
                    }
                }

                // Kiểm soát quả bóng: Va chạm với thanh máy
                if (ball.Location.Y < paddle2.Location.Y + paddle2.Height && ball.Location.X > (int)(paddle2.Location.X - ball.Width / 2) && ball.Location.X + ball.Width < (int)(paddle2.Location.X + paddle.Width + ball.Width / 2) && ball.Location.Y > (int)(paddle2.Location.Y + paddle2.Height / 2))
                {
                    yspeed *= -1;
                    xspeed = Math.Abs(paddle.Location.X - lastx_cpu);
                    if (xspeed > 4)
                    {
                        xspeed = 4;
                    }
                    else if (xspeed < -4)
                    {
                        xspeed = -4;
                    }
                    else if (xspeed == 0)
                    {
                        Random a = new Random();
                        if (a.NextDouble() > .5)
                        {
                            xspeed = 2;
                        }
                        else
                        {
                            xspeed = -2;
                        }
                    }
                    while (ball.Location.Y - 1 < paddle2.Location.Y + paddle2.Height)
                    {
                        ball.Location = new Point(ball.Location.X, ball.Location.Y + 1);
                    }
                }

                // Kiểm soát quả bóng: Máy ghi điểm
                if (ball.Location.Y > bottomBounds)
                {
                    ball.Location = new Point(120, 100);
                    Random b = new Random();
                    if (b.NextDouble() > .5)
                    {
                        xspeed = 2;
                    }
                    else
                    {
                        xspeed = -2;
                    }
                    yspeed = -2;
                    score_cpu++;
                    points2.Text = "CPU: " + score_cpu;

                    // Kiểm tra điểm số
                    if (score_cpu >= 5)
                    {
                        MessageBox.Show("Bạn đã thua!");
                        ResetGame();
                    }
                }
                // Kiểm soát quả bóng: Người chơi ghi điểm
                else if (ball.Location.Y < topBounds)
                {
                    ball.Location = new Point(120, 100);
                    Random b = new Random();
                    if (b.NextDouble() > .5)
                    {
                        xspeed = 2;
                    }
                    else
                    {
                        xspeed = -2;
                    }
                    yspeed = 2;
                    score_player++;
                    points1.Text = "Người chơi: " + score_player;

                    // Kiểm tra điểm số
                    if (score_player >= 5)
                    {
                        MessageBox.Show("Bạn đã thắng!");
                        ResetGame();
                    }
                }

                lastx = MousePosition.X;
                lastx_cpu = paddle2.Location.X;
                // Update Q-learning
                currentState = ball.Location.X * 1000 + paddle.Location.X;
                int action = ChooseAction(currentState);
                paddle2.Location = new Point(paddle2.Location.X + action, paddle2.Location.Y);

                if (previousState != 0)
                {
                    double reward = (currentState > rightBounds) ? -1.0 : 0.0;
                    double maxQ = QTable[currentState].Values.Max();
                    QTable[previousState][action] += learningRate * (reward + discountFactor * maxQ - QTable[previousState][action]);

                    // In ra giá trị của QTable
                    Console.WriteLine("QTable:");
                    Console.WriteLine($"Ball Location X: {ball.Location.X}");
                    Console.WriteLine($"Paddle Location X: {paddle.Location.X}");
                    Console.WriteLine($"Paddle2 Location: {paddle2.Location}");
                    Console.WriteLine($"Action: {action}");
                    Console.WriteLine($"MaxQ: {maxQ}");
                    Console.WriteLine($"Reward: {reward}");
                    Console.WriteLine($"currentState: {currentState}");
                    Console.WriteLine($"Previous State: {previousState}");
                   
                }

                previousState = currentState;

            }
        }
        //currentState: Tính toán trạng thái hiện tại của môi trường dựa trên vị trí của quả bóng và thanh điều khiển người chơi.

        //action: Sử dụng hàm ChooseAction để chọn hành động cho thanh điều khiển máy dựa trên trạng thái hiện tại và giá trị QTable.
        //Cập nhật vị trí của thanh điều khiển máy: Dựa vào hành động được chọn, cập nhật vị trí của thanh điều khiển máy.

        //reward: Xác định phần thưởng (reward) dựa trên kết quả của hành động trước đó. Nếu quả bóng vượt qua biên phải màn hình, reward sẽ là -1, ngược lại sẽ là 0.


        //maxQ: Lấy giá trị Q lớn nhất từ QTable cho trạng thái hiện tại.

        //Cập nhật Q-value: Sử dụng công thức của thuật toán Q-learning để cập nhật giá trị Q cho cặp trạng thái-hành động trước đó. Công thức này tính toán sự khác biệt giữa giá trị Q cũ và giá trị mục tiêu (reward +
        //* maxQ), sau đó điều chỉnh giá trị Q theo tỷ lệ học(learningRate).

        //previousState: Cập nhật trạng thái trước đó để sử dụng trong vòng lặp tiếp theo.
        

        private void ResetGame()
        {
            // Đặt lại điểm số và vị trí các đối tượng
            score_player = 0;
            score_cpu = 0;
            points1.Text = "Người chơi: 0";
            points2.Text = "CPU: 0";
            ball.Location = new Point(120, 100);
            paddle.Location = new Point((this.Width - paddle.Width) / 2, this.Height - paddle.Height - 40);
            paddle2.Location = new Point((this.Width - paddle.Width) / 2, 20);

            // Đặt lại trạng thái tạm dừng
            paused = false;
            pause_txt.Visible = false;
        }
        // Di chuyển thanh điều khiển trong mỗi frame
        private void movePaddles(object sender, EventArgs e)
        {
            // Đặt vị trí chính xác của các thanh điều khiển
            paddle.Location = new Point(paddle.Location.X, bottomBounds - 46);
            paddle2.Location = new Point(paddle2.Location.X, topBounds + 12);
            pause_txt.Location = new Point((int)rightBounds / 2 - pause_txt.Width / 2, (int)bottomBounds / 2 - pause_txt.Height / 2);
        }

        // Tạm dừng game bằng cách nhấp chuột
        private void pause(object sender, EventArgs e)
        {
            // Chuyển đổi trạng thái tạm dừng khi nhấp chuột
            if (!paused)
            {
                paused = true;
                pause_txt.Visible = true;
            }
            else
            {
                paused = false;
                pause_txt.Visible = false;
            }
        }
        private int ChooseAction(int state)
        {
            if (!QTable.ContainsKey(state))
            {
                QTable[state] = new Dictionary<int, double>();
                for (int i = -5; i <= 5; i++)
                {
                    QTable[state][i] = 0.0;
                }
            }

            if (random.NextDouble() < 0.1)
            {
                return random.Next(-5, 6);
            }
            else
            {
                return QTable[state].OrderByDescending(x => x.Value).First().Key;
            }
        }

        // Double Buffer (hàm cần thiết)
        protected override void OnPaint(PaintEventArgs pe)
        {
            // Phương thức này trống không (đã được chú thích), chủ yếu để bật Double Buffering
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Lấy mức độ khó từ combo box và cài đặt tốc độ tương ứng
            string selectedDifficulty = comboBox1.SelectedItem.ToString();
            SetInitialSpeed(selectedDifficulty);
        }
        private void SetInitialSpeed(string difficulty)
        {
            switch (difficulty)
            {
                case "Dễ":
                    xspeed = 8;
                    yspeed = 8;
                    break;
                case "Vừa":
                    xspeed = 10;
                    yspeed = 10;
                    break;
                case "Khó":
                    xspeed = 12;
                    yspeed = 12;
                    break;
                default:
                    // Mặc định là mức độ "Dễ"
                    xspeed = 8;
                    yspeed = 8;
                    break;
            }
        }
    }
}
