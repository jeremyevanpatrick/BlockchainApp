//generate random transaction data
var transactions = [];
for (var x = 0; x < 1000; x++) {

	var uuid = 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
		var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
		return v.toString(16);
	});

	var toAccount = Math.round(Math.random() * 1E16);
	var fromAccount = Math.round(Math.random() * 1E16);
	var amt = Math.floor(Math.random() * (1000 - 100 + 1) + 100);
	var timestamp = '/Date(' + new Date(parseInt("16" + Math.floor(Math.random() * (495 - 100 + 1) + 100) + "13746000")).getTime() + ')/';

	transactions.push({
		"TransactionId": uuid.toString(),
		"TransactionNumber": x.toString(),
		"ToAccount": toAccount.toString(),
		"FromAccount": fromAccount.toString(),
		"Amount": amt.toString(),
		"Timestamp": timestamp,
		"Notes": "Sending $" + amt + " from " + fromAccount + " to " + toAccount
	});

}
console.log(transactions);
copy(transactions);