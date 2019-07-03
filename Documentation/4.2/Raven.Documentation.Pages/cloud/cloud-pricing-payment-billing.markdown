#Cloud: Pricing, Payment and Billing
---

{NOTE: }
* In this page:  
  * [Pricing](../cloud/cloud-pricing-payment-billing#pricing)  
     - [Additional Expanses](../cloud/cloud-pricing-payment-billing#additional-expanses)  
  * [Payment](../cloud/cloud-pricing-payment-billing#payment)  
  * [Billing](../cloud/cloud-pricing-payment-billing#billing)  
    - [Charging Failures](../cloud/cloud-pricing-payment-billing#charging-failures)  
{NOTE/}

{WARNING: }
Prices and fees in screenshots that appear in this documentation are for illustration only.  
{WARNING/}

---

{PANEL: Pricing}

You can pay for your instances by one of three payment models: On-Demand, Yearly with _no_ Upfront payment, 
or Yearly _with_ an Upfront payment.  

* **On-Demand Payment**  
  The on-demand model lets you pay for your instance by the minute, and come and go as you please.  
  We will measure your instance run-time from rise to termination, and charge you with minute granularity.  
  You may find this model useful because it is the most flexible one. You aren't tied to a particular configuration
  and can shift between them at will. 

   - Charge is **by the minute**, with a **minimum-limit of one hour**.  
     If you run an instance for 93 minutes for example, you will pay for exactly 93 minutes.  
     If you run it for less than an hour, say 42 minutes, you'll still be charged for a full hour worth to match the minimum limit.  
   - What then is the actual price per minute?  
     While you [provision or edit](../cloud/cloud-control-panel#the-products-tab) your product, 
     costs are shown in the **Your Order** slot.  
     The price is an outcome of your HW and pricing model. 
     a 2-Cores/0.5GB-RAM instance would naturally cost less than an 8-Cores/32GB-RAM instance.  
      - A basic Production configuration:  
        ![PB0](images/pricing_001_PB0.png "PB0")
      - A little more advanced Production configuration:  
        ![PB1](images/pricing_002_PB1.png "PB1")
     You can also find the cost in the [summary page] before running the instance, and in your Billing tab.  

* **Yearly Contract**  
  You can choose one of two yearly-payment models. Both provide you with a discount in relation to the on-demand model.
  However, once a yearly contract is active, it is charged fully to the end of the year. You cannot change a yearly 
  contract. 

   * **Yearly with no upfront payment**  
     You can commit for a year and **pay monthly**, and get a **5% discount**.  
     ![Yearly No Upfront](images/pricing-003-payment-models-yearly-no-upfront.png "Yearly No Upfront")

   * **Yearly with an upfront payment**  
     You can commit for a year and **pay for it all in advance**, and get a **10% discount**.  
     ![Yearly Upfront](images/pricing-004-payment-models-yearly-upfront.png "Yearly Upfront")

  {NOTE: }
   Yearly contracts are relevant only for [production instances](../cloud/cloud-instances#a-production-instance).  
   [Development instances](../cloud/cloud-instances#a-development-instance) can only use the on-demand model.  
  {NOTE/}
  

  {NOTE: }
   You **cannot** cancel a yearly plan. Terminating your product will **not** stop your monthly payments.  
   Upscaling or downscaling a product will keep your old product and add the new one to your account.  
   
   Adding nodes to an existing cluster with a yearly contract, however, does not incur additional expenses 
   beyond the cost of running the new nodes.  
  {NOTE/}

  ---

####Additional Expanses

Your pricing model does **not** cover incidental expanses over -  

* **Traffic Usage**  
  Your charges for traffic to and from your instance will be added to your basic pricing plan.  
* **Expanding your Disk Storage**  
  We will automatically expand the amount of disk space allocated for your product when its usage reaches 90%, 
  to prevent any chance of data loss. The new disk size will remain (it won't shrink back to its initial size if 
  you delete your data), and your payment plan will be updated accordingly.  
* **Backup Storage**  
  Your RavenDB Cloud runs automatic backups on a regular schedule.  
  Depending on your retention settings, you may be charged for highly available storage of your backups.  
   - Your plan includes a 1GB / month backup storage. 

{PANEL/}

{PANEL: Payment}

You can currently pay by credit card or wire transfer.  

* Credit Card  
  You can provide your credit card details while creating your account, or skip this stage and return to it later.  
  To provide your credit card details at any time, enter your Account tab and click the Add Credit Card button.  
  Select your main credit card using the Active button, so we knoe which card to try first.  
  If charging your active card fails, we'll try to charge your other cards.  

  ![Payment](images/payment.png "Payment")

* Wire Transfer / Purchase Order
  Aproach our Support personnel to use wire transfer or purchase order.

{PANEL/}

{PANEL: Billing}

Your Control Panel's [Billing tab](../cloud/cloud-control-panel#the-billing-tab) summarizes your 
outstanding charges and past invoices.

Additional data that you can see in the billing tab includes Daily cost, Total cost, and the expected 
charge in the end of this month.  

####Charging failures  

If we fail to charge your account using the payment method you provided, we'll notify 
you and retry for 7 days. Failure to pay after that period may result in account closure. 

{PANEL/}
