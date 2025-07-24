document.addEventListener('DOMContentLoaded', function () {
    const deliverySelect = document.getElementById('deliveryMethod');
    const deliveryFeeSpan = document.getElementById('deliveryFee');
    const totalPriceSpan = document.getElementById('totalWithDelivery');
    const productsTotal = parseFloat('@productsTotalStr');

    const deliveryFees = {
        1: parseFloat('@econtFeeStr'),
        2: parseFloat('@speedyFeeStr'),
        3: parseFloat('@toAddressFeeStr')
    };

    deliverySelect.addEventListener('change', function () {
        const selectedMethod = parseInt(this.value);
        const deliveryFee = deliveryFees[selectedMethod] || 0;
        const total = productsTotal + deliveryFee;

        deliveryFeeSpan.textContent = deliveryFee.toFixed(2);
        totalPriceSpan.textContent = total.toFixed(2);
    });
});
