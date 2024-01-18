document.getElementById("tokenLink").addEventListener("click",
    function(event) {
        event.preventDefault();

        fetch("/Transaction/Auth",
                {
                    method: "POST",
                    headers: {
                        'Content-Type': "application/json"
                    },
                    body: JSON.stringify({})
                })
            .then(response => response.json())
            .then(data => {
                console.log(data);
            })
            .catch(error => {
                console.error(error);
            });
    });