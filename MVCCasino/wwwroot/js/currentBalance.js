function updateCurrentBalance() {
    $.ajax({
        type: "GET",
        url: "api/Transaction/balance",
        success: function (data) {
            $('#currentBalance').text('Current Balance: ' + data.currentBalance);
            console.log('updateBalance js update balance success');
        },
        error: function (error) {
            console.error("Failed to fetch current balance", error);
        }
    });
}

setInterval(updateCurrentBalance, 30000);
updateCurrentBalance();
