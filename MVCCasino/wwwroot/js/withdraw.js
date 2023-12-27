function withdraw() {
    // You may want to show a modal or form to gather withdraw details from the user
    let withdrawalAmount = prompt("Enter withdrawal amount:");

    if (withdrawalAmount !== null && withdrawalAmount !== "") {
        console.log("Withdrawal Amount: " + withdrawalAmount);
        $.ajax({
            type: "POST",
            url: "/api/wallet/withdraw",
            success: function (data) {
                // Handle success, e.g., update UI
                console.log("Withdraw successful");
            },
            error: function (error) {
                // Handle error
                console.error("Withdraw failed", error);
            }
        });
    } else {
        console.log("Withdrawal canceled or invalid input");
    }
}