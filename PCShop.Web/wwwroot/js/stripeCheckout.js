document.addEventListener('DOMContentLoaded', function () {
    const stripePublicKey = '@Model.StripePublicKey';
    const checkoutButton = document.getElementById('checkout-button');
    const buttonText = document.getElementById('button-text');

    // Get anti-forgery token from the form
    const token = document.querySelector('input[name="__RequestVerificationToken"]').value;

    checkoutButton.disabled = false;
    buttonText.textContent = 'Pay Now';

    checkoutButton.addEventListener('click', function () {
        checkoutButton.disabled = true;
        buttonText.textContent = 'Processing...';

        const paymentData = {
            amount: @totalPrice.ToString(System.Globalization.CultureInfo.InvariantCulture),
            fullName: '@fullNameEscaped',
            email: '@emailEscaped'
        };

        fetch('@Url.Action("CreateCheckoutSession", "Order")', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': token // <-- add CSRF token here
            },
            body: JSON.stringify(paymentData)
        })
            .then(response => {
                if (!response.ok) {
                    throw new Error(`HTTP error! status: ${response.status}`);
                }
                return response.json();
            })
            .then(session => {
                if (session.id) {
                    const stripe = Stripe(stripePublicKey);
                    return stripe.redirectToCheckout({ sessionId: session.id });
                } else {
                    throw new Error('No session ID returned from server');
                }
            })
            .catch(error => {
                alert('There was an error processing your payment: ' + error.message);
                checkoutButton.disabled = false;
                buttonText.textContent = 'Pay Now';
            });
    });
});