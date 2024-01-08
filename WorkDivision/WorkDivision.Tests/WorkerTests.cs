namespace WorkDivision.Tests
{
    public class WorkerTests: IDisposable
    {
        private readonly Worker _worker;

        public WorkerTests()
        {
            _worker = new Worker();
        }

        [Theory]
        [InlineData(new object[] {1,10})]
        [InlineData(new object[] {2,20})]
        [InlineData(new object[] {3,30})]
        public void GetBonusPercent_by_rank(int rank, int expected)
        {
            //Arrange


            //Act
            var result = _worker.GetBonusPercent(rank);

            //Assert
            Assert.Equal(expected,result);
        }

        [Theory]
        [InlineData(new object[] { -1, 0 })]
        public void GetBonusPercent_get_default_value_by_unreal_rank(int rank, int expected)
        {
            //Arrange


            //Act
            var result = _worker.GetBonusPercent(rank);

            //Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void CanBeTheBoss_should_throw_exeption_by_unreal_rank()
        {
            //Arrange
            _worker._rank = 100;

            //Act
            var result = _worker.CanBeTheBoss(_worker._rank);

            //Assert
            Assert.Throws<ArgumentException>(()=> result);
        }


        public void Dispose()
        {

        }
    }
}