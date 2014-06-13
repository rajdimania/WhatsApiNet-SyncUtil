WhatsApiNet-SyncUtil
====================

WhatsApp contact sync tool using WhatsApiNet library


```
Usage:
        wasyncutil.exe username=PHONENUMBER password=PASSWORD (mode=MODE) (debug=true) (context=CONTEXT) number1 number2 ...

Example: wasyncutil.exe username=31612345678 password=asdasdasASDASDasd== 3161234567 31576454543 316453634

Options:
        mode: sync mode (full or delta)
        context: sync context (registration, background or interactive)
        debug: enables debug output (true)

Returns:
        existing phone numbers (delimited by line breaks)
```

Todo:
- [ ] Integrate WhatsAppApi.dll into assembly
- [ ] Support for CSV files
- [ ] Support for Google contacts
- [ ] GUI
