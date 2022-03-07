# REST API example application

This is a application providing a REST API 
to a Metal managming model.

The entire application is contained within the `Parcer/Programm.cs` file.

`Parcer/Properties/launchSettings.json` is a minimal configuration.

# REST API

The REST API to the example app is described below.

## Get list of Certificats

### Request

`GET /certificate/`

    curl -i -H 'Accept: application/json' https://localhost:7109/certificate/

### Response

    HTTPS/1.1 200 OK
    Date: Mon, 07 Mar 2022 10:42:53 GMT
    Status: 200 OK
    Connection: close
    Content-Type: application/json

	{
		"certificateId": 1,
		"link": "https://doc.nlmk.shop/c?q=17RlwEUbyASJ1Ke",
		"number": "40384 ",
		"date": "2021-07-19T00:00:00",
		"author": null,
		"authorAddress": null,
		"fax": null,
		"recipient": null,
		"recipientCountry": null,
		"contract": null,
		"specificationNumber": null,
		"product": {
			"productId": 1,
			"name": "Сталь холоднокатаная конструкционная",
			"labeling": null,
			"code": null
		},
		"shipmentShop": "13Цех холодного проката и покрытий",
		"wagonNumber": "95535399",
		"orderNumber": "40452579-2021",
		"typeOfRollingStock": "Спецплатформа",
		"typeOfPackaging": null,
		"placeNumber": null,
		"gosts": null,
		"notes": "№договора:203349-21",
		"packages": [
			{
				"packageId": 1,
				"dateAdded": "2022-03-07T03:11:19.002819",
				"dateChange": "2022-03-07T03:11:19.0029465",
				"status": {
					"statusId": 3,
					"statusName": "Имеется"
				},
				"namberConsignmentPackage": "1",
				"heat": "2164623",
				"batch": "8157999-2",
				"orderPosition": null,
				"numberOfClientMaterial": null,
				"serialNumber": null,
				"grade": "08Ю",
				"category": null,
				"strengthGroup": null,
				"profile": null,
				"barcode": null,
				"size": {
					"sizeId": 1,
					"thickness": 1.2,
					"width": 1230,
					"length": null
				},
				"quantity": 1,
				"variety": "1 сорт",
				"gost": "ГОСТ 9045-93",
				"weight": {
					"weightId": 1,
					"gross": 8.08,
					"gross2": null,
					"net": 7.99
				},
				"customerItemNumber": null,
				"treatment": null,
				"groupCode": null,
				"pattemCutting": null,
				"surfaceQuality": "II",
				"rollingAccuracy": null,
				"categoryOfDrawing": "ВГ",
				"stateOfMatirial": null,
				"roughness": null,
				"flatness": null,
				"trimOfEdge": "О",
				"weldability": null,
				"orderFeatures": null,
				"chemicalComposition": {
					"chemicalCompositionId": 1,
					"c": 0.04,
					"mn": 0.24,
					"si": 0.012,
					"s": 0.008,
					"p": 0.017,
					"cr": 0.03,
					"ni": 0.02,
					"cu": 0.04,
					"as": null,
					"n2": 0.005,
					"al": 0.03,
					"ti": 0.001,
					"mo": null,
					"w": null,
					"v": 0.001,
					"alWithN2": null,
					"cev": null,
					"notes": null
				},
				"sampleLocation": null,
				"directOfTestPicses": null,
				"temporalResistance": 335,
				"yieldPoint": null,
				"tensilePoint": null,
				"elongation": 37,
				"bend": null,
				"hardness": null,
				"rockwell": null,
				"brinel": null,
				"eriksen": null,
				"impactStrength": null,
				"grainSize": null,
				"decarburiization": null,
				"cementite": null,
				"banding": null,
				"corrosion": null,
				"testingMethod": null,
				"unitTemporaryResistance": null,
				"unitYieldStrength": null,
				"sphericalHoleDepth": 11.4,
				"microBallCem": 0,
				"r90": 0,
				"n90": 0,
				"koafNavodorag": 0,
				"notes": null,
				"photo": null,
				"comment": null
			},
		]
	}

## Create a new Certificate by link

### Request

`POST /`

    curl -i -H 'Accept: application/json' http://localhost:7109/
	
	{
		"link":"absolutePath"
	}

### Response

    HTTP/1.1 201 Created
    Date: Mon, 07 Mar 2022 10:42:53 GMT
    Status: 201 Created
    Connection: close
    Content-Type: application/json
    Location: Parcer/CreateAsync

    id:1

## Get a specific Certificate

### Request

`GET /certificate/id`

    curl -i -H 'Accept: application/json' http://localhost:7109/certificate/1

### Response

    HTTP/1.1 200 OK
    Date: Mon, 07 Mar 2022 10:42:53 GMT
    Status: 200 OK
    Connection: close
    Content-Type: application/json

    {
    "certificateId": 1,
    "link": "https://doc.nlmk.shop/c?q=17RlwEUbyASJ1Ke",
    "number": "40384 ",
    "date": "2021-07-19T00:00:00",
    "author": null,
    "authorAddress": null,
    "fax": null,
    "recipient": null,
    "recipientCountry": null,
    "contract": null,
    "specificationNumber": null,
    "product": {
        "productId": 1,
        "name": "Сталь холоднокатаная конструкционная",
        "labeling": null,
        "code": null
    },
    "shipmentShop": "13Цех холодного проката и покрытий",
    "wagonNumber": "95535399",
    "orderNumber": "40452579-2021",
    "typeOfRollingStock": "Спецплатформа",
    "typeOfPackaging": null,
    "placeNumber": null,
    "gosts": null,
    "notes": "№договора:203349-21",
    "packages": [
        {
            "packageId": 1,
            "dateAdded": "2022-03-07T03:11:19.002819",
            "dateChange": "2022-03-07T03:11:19.0029465",
            "status": {
                "statusId": 3,
                "statusName": "Имеется"
            },
            "namberConsignmentPackage": "1",
            "heat": "2164623",
            "batch": "8157999-2",
            "orderPosition": null,
            "numberOfClientMaterial": null,
            "serialNumber": null,
            "grade": "08Ю",
            "category": null,
            "strengthGroup": null,
            "profile": null,
            "barcode": null,
            "size": {
                "sizeId": 1,
                "thickness": 1.2,
                "width": 1230,
                "length": null
            },
            "quantity": 1,
            "variety": "1 сорт",
            "gost": "ГОСТ 9045-93",
            "weight": {
                "weightId": 1,
                "gross": 8.08,
                "gross2": null,
                "net": 7.99
            },
            "customerItemNumber": null,
            "treatment": null,
            "groupCode": null,
            "pattemCutting": null,
            "surfaceQuality": "II",
            "rollingAccuracy": null,
            "categoryOfDrawing": "ВГ",
            "stateOfMatirial": null,
            "roughness": null,
            "flatness": null,
            "trimOfEdge": "О",
            "weldability": null,
            "orderFeatures": null,
            "chemicalComposition": {
                "chemicalCompositionId": 1,
                "c": 0.04,
                "mn": 0.24,
                "si": 0.012,
                "s": 0.008,
                "p": 0.017,
                "cr": 0.03,
                "ni": 0.02,
                "cu": 0.04,
                "as": null,
                "n2": 0.005,
                "al": 0.03,
                "ti": 0.001,
                "mo": null,
                "w": null,
                "v": 0.001,
                "alWithN2": null,
                "cev": null,
                "notes": null
            },
            "sampleLocation": null,
            "directOfTestPicses": null,
            "temporalResistance": 335,
            "yieldPoint": null,
            "tensilePoint": null,
            "elongation": 37,
            "bend": null,
            "hardness": null,
            "rockwell": null,
            "brinel": null,
            "eriksen": null,
            "impactStrength": null,
            "grainSize": null,
            "decarburiization": null,
            "cementite": null,
            "banding": null,
            "corrosion": null,
            "testingMethod": null,
            "unitTemporaryResistance": null,
            "unitYieldStrength": null,
            "sphericalHoleDepth": 11.4,
            "microBallCem": 0,
            "r90": 0,
            "n90": 0,
            "koafNavodorag": 0,
            "notes": null,
            "photo": null,
            "comment": null
        },
    ]
}

## Get a non-existent Thing

### Request

`GET /certificate/id`

    curl -i -H 'Accept: application/json' http://localhost:7109/certificate/9999

### Response

    HTTP/1.1 204 No Content
    Date: Mon, 07 Mar 2022 10:42:53 GMT
    Status: 404 Not Found
    Connection: close
    Content-Type: application/json

    {"status":204,"reason":"No Content"}