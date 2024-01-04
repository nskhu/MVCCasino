function updateCurrentBalance() {
    $.ajax({
        type: "GET",
        url: "api/Transaction/balance",
        success: function (data) {
            $('#currentBalance').text('Current Balance: ' + data.currentBalance);
        },
        error: function (error) {
            console.error("Failed to fetch current balance", error);
        }
    });
}

setInterval(updateCurrentBalance, 30000);
updateCurrentBalance();
