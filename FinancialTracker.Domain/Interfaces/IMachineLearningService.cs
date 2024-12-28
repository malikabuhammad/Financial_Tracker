using FinancialTracker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialTracker.Domain.Interfaces
{
    public interface IMachineLearningService
    { // Method to predict spending based on user data
        Task<float> PredictSpendingAsync(SpendingData input);
    }
}
