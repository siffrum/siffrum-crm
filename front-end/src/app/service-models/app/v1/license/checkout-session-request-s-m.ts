import { PaymentModeSM } from "../../enums/payment-mode-s-m.enum";

export class CheckoutSessionRequestSM {
    productId!: string;
    priceId!: string;
    successUrl!: string;
    failureUrl!: string;
    paymentMode!: PaymentModeSM;
}
