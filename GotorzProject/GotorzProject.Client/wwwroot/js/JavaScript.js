var stripe;
var elemets;

function initializeStripe(publicKey)
{
    stripe = Stripe(publicKey);
    elements = stripe.elements();

    var style = {
        base: {
            color: "#32325d",
            fontFamily: '"Helvetica Neue", Helvetica, sans-serif',
            fontSmoothing: "antialiased",
            fontSize: "16px",
            "::placeholder":
            {
                color: "#aab7c4"
            }
        }
    };


    var card = elements.create("card", { style: style });
    card.mount("#card-element");

    var form = document.getElementById("payment-form");
    form.addEventListener("submit", function (event) {
        event.preventDefault();

        stripe
            .createToken(card)
            .then(function (result) {
                if (result.error) {
                    console.log(result.error.message);

                }
                else {
                    submitTokenToServer(result.token);
                }
            });

    });

}

function submitTokenToServer(token)
{
    fetch("/payment/charge",
        {
            method: "POST",
            headers:
            {
                "Content-Type": "application/json",
            },
            body: JSON.stringify({ token: token.id })
        })
        .then(response => response.json())
        .then(data => {
            if (data.succes) {
                alert("Betaling gennemført");
            }
            else {
                alert("Fejl ved betaling");
            }
        });
    }
    
