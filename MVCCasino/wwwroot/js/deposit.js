function deposit() {
    let depositAmount = prompt("Enter deposit amount:");

    if (depositAmount !== null && depositAmount !== "") {
        console.log("Deposit Amount: " + depositAmount);
        $.ajax({
            type: "POST",
            url: "/api/wallet/deposit",
            success: function (data) {
                // Handle success, e.g., update UI
                console.log("Deposit successful");
            },
            error: function (error) {
                // Handle error
                console.error("Deposit failed", error);
            }
        })
    } else {
        console.log("Deposit canceled or invalid input");
    }
}
