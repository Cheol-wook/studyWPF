using Caliburn.Micro;
using OxyPlot;
using OxyPlot.Legends;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using WpfSmartHomeMonitoringApp.Helpers;
using WpfSmartHomeMonitoringApp.Models;

namespace WpfSmartHomeMonitoringApp.ViewModels
{
    class HistoryViewModel : Screen
    {
        private BindableCollection<DivisionModel> divisions;
        private DivisionModel selectedDivision;
        private string startDate;
        private string initStartDate;
        private string endDate;
        private string initEndDate;
        private int totalCount;
        private PlotModel historyModel;     //smartHomeModle -> historyModel 이름 변경 2022.06.13


        /*
        Divisions
        DivisionVal
        SelectedDivision
        StartDate
        InitStartDate
        EndDate
        InitEndDate
        TotalCount
        SearchIoTData()
        historyModel
            */
        public BindableCollection<DivisionModel> Divisions
        {
            get { return divisions; }
            set
            {
                divisions = value;
                NotifyOfPropertyChange(() => Divisions);
            }
        }
        public DivisionModel SelectedDivision
        {
            get { return selectedDivision; }
            set
            {
                selectedDivision = value;
                NotifyOfPropertyChange(() => SelectedDivision);
            }
        }
        public string StartDate
        {
            get { return startDate; }
            set
            {
                startDate = value;
                NotifyOfPropertyChange(() => StartDate);
            }
        }
        public string InitStartDate
        {
            get { return initStartDate; }
            set
            {
                initStartDate = value;
                NotifyOfPropertyChange(() => InitStartDate);
            }
        }
        public string EndDate
        {
            get { return endDate; }
            set
            {
                endDate = value;
                NotifyOfPropertyChange(() => EndDate);
            }
        }
        public string InitEndDate
        {
            get { return initEndDate; }
            set
            {
                initEndDate = value;
                NotifyOfPropertyChange(() => InitEndDate);
            }
        }
        public int TotalCount
        {
            get { return totalCount; }
            set
            {
                totalCount = value;
                NotifyOfPropertyChange(() => TotalCount);
            }
        }
        public PlotModel HistoryModel       //smartHomeModle -> historyModel 이름 변경 2022.06.13
        {
            get { return historyModel; }
            set
            {
                historyModel = value;
                NotifyOfPropertyChange(() => HistoryModel);
            }
        }
        public HistoryViewModel()
        {
            Commons.CONNSTRING = "Data Source=PC01;Initial Catalog=OpenApiLab;Integrated Security=True";
            InitControl();
        }

        private void InitControl()
        {
            Divisions = new BindableCollection<DivisionModel>   // 콤보박스용 데이터 생성
            {
                new DivisionModel
                {
                    KeyVal = 0,
                    DivisionVal = "-- Select --"
                },
                new DivisionModel
                {
                    KeyVal = 1,
                    DivisionVal = "DINNING"
                },
                new DivisionModel
                {
                    KeyVal = 2,
                    DivisionVal = "LIVING"
                },
                new DivisionModel
                {
                    KeyVal = 3,
                    DivisionVal = "BED"
                },
                new DivisionModel
                {
                    KeyVal = 4,
                    DivisionVal = "BATH"
                }
            };
            // select를 선택해서 초기화
            SelectedDivision = Divisions.Where(v => v.DivisionVal.Contains("Select")).FirstOrDefault();

            InitStartDate = DateTime.Now.ToShortDateString(); //2022-06-10
            InitEndDate = DateTime.Now.AddDays(1).ToShortDateString(); //2022-06-11
        }

        //검색 메서드
        public void SearchIoTData()
        {
            // Vaildation check
            if (SelectedDivision.KeyVal == 0)
            {
                MessageBox.Show("검색할 방을 선택하세요!");
                return;
            }
            if(DateTime.Parse(StartDate) > DateTime.Parse(EndDate))
            {
                MessageBox.Show("시작 날짜가 종료 날짜보다 최신일 수 없습니다.");
                return;
            }

            TotalCount = 0;

            using (SqlConnection conn = new SqlConnection(Commons.CONNSTRING))
            {
                string strQuery = @"SELECT Id, CurrTime, Temp, Humid
                                        FROM TblSmartHome
                                        WHERE DevId = @DevId
                                        AND CurrTime BETWEEN @StartDate AND @EndDate
                                        ORDER BY Id ASC";

                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(strQuery, conn);

                    SqlParameter parmDevId = new SqlParameter("@DevId", selectedDivision.DivisionVal);
                    cmd.Parameters.Add(parmDevId);
                    SqlParameter parmStartDate = new SqlParameter("@StartDate", StartDate);
                    cmd.Parameters.Add(parmStartDate);
                    SqlParameter parmEndDate = new SqlParameter("@EndDate", EndDate);
                    cmd.Parameters.Add(parmEndDate);

                    SqlDataReader reader = cmd.ExecuteReader();

                    var i = 0;
                    //start of chart process 2022.06.13 추가

                    PlotModel tmp = new PlotModel { Title = $"{SelectedDivision.DivisionVal} Histories"} ;      // 임시 플롯모델
                    LineSeries seriesTemp = new LineSeries 
                    {
                        Color = OxyColor.FromRgb(180,20,50), 
                        Title = "Temperature", MarkerType = MarkerType.Circle,
                        MarkerSize = 3
                    };      // 온도 값을 라인차트에 넣을 객체
                    LineSeries seriesHumid = new LineSeries
                    {
                        Color = OxyColor.FromRgb(150, 150, 255),
                        Title = "Humidity",
                        MarkerType = MarkerType.Triangle
                    };

                    while (reader.Read())
                    {
                        //var temp = reader["Temp"];
                        // Temp, Humid 차트 데이터 생성
                        seriesTemp.Points.Add(new DataPoint(i, Convert.ToDouble(reader["Temp"])));
                        seriesHumid.Points.Add(new DataPoint(i, Convert.ToDouble(reader["Humid"])));
                        i++;
                    }

                    TotalCount = i;     // 검색한 데이터 총 개수

                    tmp.Series.Add(seriesTemp);
                    tmp.Series.Add(seriesHumid);

                    HistoryModel = tmp;

                    //end of chart process 2022.06.13
                }
                catch (Exception ex)
                {

                    MessageBox.Show($"Error {ex.Message}");
                    return;
                }
            }
        }

    }
}
