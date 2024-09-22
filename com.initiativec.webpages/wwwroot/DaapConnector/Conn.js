import {
    getProtocolParameters,
    buf2Hex,
    hex2Bytes,
    selectUtxo,
    reportError,
    ERROR,
    NETWORK
} from './util.js';

import {
    Address,
    BaseAddress,
    MultiAsset,
    Assets,
    ScriptHash,
    Costmdls,
    Language,
    CostModel,
    AssetName,
    TransactionUnspentOutput,
    Value,
    //TransactionBuilderConfigBuilder,
    TransactionBuilder,
    LinearFee,
    BigNum,
    BigInt,
    TransactionHash,
    TransactionInputs,
    TransactionInput,
    TransactionWitnessSet,
    Transaction,
    PlutusData,
    PlutusScripts,
    PlutusScript,
    PlutusList,
    Redeemers,
    Redeemer,
    RedeemerTag,
    Ed25519KeyHashes,
    ConstrPlutusData,
    ExUnits,
    Int,
    NetworkInfo,
    EnterpriseAddress,
    TransactionOutputs,
    hash_transaction,
    hash_script_data,
    hash_plutus_data,
    ScriptDataHash, Ed25519KeyHash, NativeScript, StakeCredential
} from "./@emurgo/cardano-serialization-lib-browser/cardano_serialization_lib.js"

//import TransactionBuilderConfigBuilder from "./@emurgo/cardano-serialization-lib-browser/cardano_serialization_lib.js"
import TransactionOutput from "./@emurgo/cardano-serialization-lib-browser/cardano_serialization_lib.js"
import TransactionOutputBuilder from "./@emurgo/cardano-serialization-lib-browser/cardano_serialization_lib.js"
import TransactionUnspentOutputs from "./@emurgo/cardano-serialization-lib-browser/cardano_serialization_lib.js"

import init from './@emurgo/cardano-serialization-lib-browser/cardano_serialization_lib.js';

init('/daapconnector/@emurgo/cardano-serialization-lib-browser/cardano_serialization_lib_bg.wasm.js').then(run);



window.State = {
    selectedTabId: "1",
    whichWalletSelected: undefined,
    walletFound: false,
    walletIsEnabled: false,
    walletIsConnected: false,
    walletName: undefined,
    walletIcon: undefined,
    walletAPIVersion: undefined,
    wallets: [],

    networkId: undefined,
    Utxos: undefined,
    CollatUtxos: undefined,
    balance: undefined,
    changeAddress: undefined,
    rewardAddress: undefined,
    usedAddress: undefined,

    txBody: undefined,
    txBodyCborHex_unsigned: "",
    txBodyCborHex_signed: "",
    submittedTxHash: "",

    addressBech32SendADA: "addr_test1qrt7j04dtk4hfjq036r2nfewt59q8zpa69ax88utyr6es2ar72l7vd6evxct69wcje5cs25ze4qeshejy828h30zkydsu4yrmm",
    lovelaceToSend: 3000000,
    assetNameHex: "4c494645",
    assetPolicyIdHex: "ae02017105527c6c0c9840397a39cc5ca39fabe5b9998ba70fda5f2f",
    assetAmountToSend: 5,
    addressScriptBech32: "addr_test1wpnlxv2xv9a9ucvnvzqakwepzl9ltx7jzgm53av2e9ncv4sysemm8",
    datumStr: "12345678",
    plutusScriptCborHex: "4e4d01000033222220051200120011",
    transactionIdLocked: "",
    transactionIndxLocked: 0,
    lovelaceLocked: 3000000,
    manualFee: 900000,

    NFTPrice: undefined,
}

window.WalletAPI = null;

var protocolParams = {
    linearFee: {
        minFeeA: "44",
        minFeeB: "155381",
    },
    minUtxo: "34482",
    poolDeposit: "500000000",
    keyDeposit: "2000000",
    maxValSize: 5000,
    maxTxSize: 16384,
    priceMem: 0.0577,
    priceStep: 0.0000721,
    coinsPerUtxoWord: "34482",
}


// ================================================================= //

async function run() {

    State.whichWalletSelected = await getWalletKey();
    //console.log(State.whichWalletSelected);
    
    if (State.whichWalletSelected == null) {
        prepareButtonWalletsConnect();

        //prepareMintButton();
    }
    else {

        State.walletIsConnected = await isConnectWallet(State.whichWalletSelected);

        if (State.walletIsConnected) {
            State.walletIsEnabled = enableWallet();
            await getNetworkId();
            await getBalance();

            //console.log(State.networkId);


            getUtxos();

            //if (State.networkId != 1) {
            //    alert('NETWORK WRONG');
            //    return false;
            //    //window.location.href = '/';
            //}

            if (State.walletIsEnabled) {
                //console.log('Connected');
                

                prepareMintButtonLoad();
                

                var notConnected = document.querySelector('#js-btn-connect-wallet');
                notConnected.style.cssText = 'display:none;';

                var loadBtnLoad = document.querySelector('#js-btn-connect-load'); 
                loadBtnLoad.style.cssText = 'display:block;';

                var dashboardButton = document.querySelector('.js-dashboard-menu');
                dashboardButton.style.cssText = 'display:block;';

                var dashboardButtonSecond = document.querySelector('.js-dashboard-menu-second');
                dashboardButtonSecond.style.cssText = 'display:block;';

                State.usedAddress = await getUsedAddresses();
                window.localStorage.setItem("WalletAddress", State.usedAddress);


                let firstPart = State.usedAddress.substring(0, 12);
                let lastPart = State.usedAddress.substring(State.usedAddress.length - 8);

                let showAddress = firstPart + "..." + lastPart;


                loadBtnLoad.style.cssText = 'display:none;';

                

                var connected = document.querySelector('#js-btn-connected');
                connected.style.cssText = 'display:block; width: 270px;';
                var txtConnected = document.querySelector('.js-btn-connected');
                var txtConnectedWallet = document.querySelector('.js-btn-connected-wallet');
                

                try {
                    let containerQttyItens = document.querySelector('#js-container-qtty-itens');
                    containerQttyItens.style.cssText = 'opacity: 1; transform: none; margin-top: 50px; z-index: 1; display:block;';
                }
                catch {

                }


                stateButtonMint();
                //console.log(showAddress);
                txtConnected.innerHTML = showAddress;

                if (txtConnectedWallet !== null) {
                    txtConnectedWallet.innerHTML = showAddress;
                }
                

                //getAPIVersion()
                //console.log(State.usedAddress);
            }
            else {
                prepareButtonWalletsConnect();
            }

        }
        else {
            prepareButtonWalletsConnect();

        }
    }
}


function prepareMintButtonLoad() {

    try {
        var labelPrincipalText = document.querySelector('#js-label-principal-text-buy');
        var labelSecundaryText = document.querySelector('#js-label-secundary-text-buy');
        var btnConnectBuy = document.querySelector('#js-btn-connect-buy');
        var spanConnectBuy = document.querySelector('#js-span-connect-buy');


        labelPrincipalText.innerHTML = "Establishing a connection";
        labelSecundaryText.innerHTML = "Please wait a moment.";


        btnConnectBuy.setAttribute("disabled", '');
        btnConnectBuy.style.cssText = 'background-color: rgb(89, 90, 99)';
        spanConnectBuy.innerHTML = "Connect...";
    } catch {

    }
    
}

function stateButtonMint() {
    try {
        var labelPrincipalText = document.querySelector('#js-label-principal-text-buy');
        var labelSecundaryText = document.querySelector('#js-label-secundary-text-buy');
        var btnConnectBuy = document.querySelector('#js-btn-connect-buy');
        var spanConnectBuy = document.querySelector('#js-span-connect-buy');


        labelPrincipalText.innerHTML = "Mint a GoodVibes";
        labelSecundaryText.innerHTML = "LOAD PRICE...";


        btnConnectBuy.setAttribute("disabled", '');
        btnConnectBuy.style.cssText = 'background-color: rgb(89, 90, 99)';
        spanConnectBuy.innerHTML = "Mint a GoodVibes";


        

        //validatinFunds(12449993344,3);

    } catch {

    }
}

function alterCurrentPriceGoodVibe(currentPrice) {
    try {

        let valueConvert = currentPrice / 1000000

        var labelSecundaryText = document.querySelector('#js-label-secundary-text-buy');
        labelSecundaryText.innerHTML = "You can mint up to 10 GoodVibes. Current Price: " + valueConvert + " ADA";
    } catch {

    }
}

function validatinFunds(valueToValidate, quantity) {

    var btnConnectBuy = document.querySelector('#js-btn-connect-buy');
    var spanConnectBuy = document.querySelector('#js-span-connect-buy');

    //console.log(valueToValidate);
    //console.log(State.balance);

    if (State.balance <= valueToValidate) {
        btnConnectBuy.setAttribute("disabled", '');
        btnConnectBuy.style.cssText = 'background-color: rgb(89, 90, 99)';
        spanConnectBuy.innerHTML = "Insufficient ADA";

        return false;
    }
    else {
        btnConnectBuy.removeAttribute("disabled");
        btnConnectBuy.style.cssText = 'background-color: rgb(142, 244, 46)';
        //spanConnectBuy.innerHTML = "Mint " + quantity + " Vibes";

        return true;
    }
}

window.ValidatingOrder = async function(qtty, sum) {
    let currentPrince = State.NFTPrice;
    let qttyNft = parseInt(qtty.text());

    if (sum) {
        qttyNft++;
    }
    else {
        qttyNft--;
    }


    let totalValue = currentPrince * qttyNft;

    //console.log(qttyNft);

    let valid = validatinFunds(totalValue, qttyNft);

    if (valid) {
        var btnConnectBuy = document.querySelector('#js-btn-connect-buy');
        var spanConnectBuy = document.querySelector('#js-span-connect-buy');


        if (qttyNft > 0) {
            btnConnectBuy.removeAttribute("disabled");
            btnConnectBuy.style.cssText = 'background-color: rgb(142, 244, 46)';
            spanConnectBuy.innerHTML = "Mint " + qttyNft + " Vibes";
        }
        else {
            btnConnectBuy.setAttribute("disabled", '');
            btnConnectBuy.style.cssText = 'background-color: rgb(89, 90, 99)';
            spanConnectBuy.innerHTML = "Mint " + qttyNft + " Vibes";
        }

        
    }
    else {
        var btnConnectBuy = document.querySelector('#js-btn-connect-buy');
        var spanConnectBuy = document.querySelector('#js-span-connect-buy');

        btnConnectBuy.setAttribute("disabled", '');
        btnConnectBuy.style.cssText = 'background-color: rgb(89, 90, 99)';
        spanConnectBuy.innerHTML = "Insufficient ADA";
    }
}


$(".js-btn-comprar").on("click", function () {

    let qtty = $('.js-label-total-mint-nft')
    let qttyNft = parseInt(qtty.text());
    CreateTransaction(State.usedAddress, qttyNft);
});



function prepareButtonWalletsConnect() {

    //console.log('prepareButtonWalletsConnect');

    const wallets = [];
    for (const key in window.cardano) {

        console.log(key);


        if (window.cardano[key].enable && wallets.indexOf(key) === -1) {

            if (key === 'nami') {

                var remove = document.querySelector('#js-connect-nami');
                remove.style.cssText = "cursor: pointer;"
                remove.id = 'nami'

                var nami = document.querySelector('#app-nami-install-wallet');
                nami.style.cssText = 'display:none;';
            }

            if (key === 'eternl') {

                var remove = document.querySelector('#js-connect-eternl');
                remove.style.cssText = "cursor: pointer;"
                remove.id = 'eternl'

                var wallet = document.querySelector('#app-eternl-install-wallet');
                wallet.style.cssText = 'display:none;';
            }

            if (key === 'typhon') {

                var remove = document.querySelector('#js-connect-typhon');
                remove.style.cssText = "cursor: pointer;"
                remove.id = 'typhon'

                var wallet = document.querySelector('#app-typhon-install-wallet');
                wallet.style.cssText = 'display:none;';
            }

            if (key === 'gerowallet') {

                var remove = document.querySelector('#js-connect-gerowallet');
                remove.style.cssText = "cursor: pointer;"
                remove.id = 'gerowallet'

                var wallet = document.querySelector('#app-gerowallet-install-wallet');
                wallet.style.cssText = 'display:none;';
            }

            if (key === 'flint') {

                var remove = document.querySelector('#js-connect-flint');
                remove.style.cssText = "cursor: pointer;"
                remove.id = 'flint'

                var wallet = document.querySelector('#app-flint-install-wallet');
                wallet.style.cssText = 'display:none;';
            }

            if (key === 'vespr') {
   
                var remove = document.querySelector('#js-connect-vespr');
                remove.style.cssText = "cursor: pointer;"
                remove.id = 'vespr'

                var wallet = document.querySelector('#app-vespr-install-wallet');
                wallet.style.cssText = 'display:none;';
            }

            if (key === 'nufi') {

                var remove = document.querySelector('#js-connect-nufi');
                remove.style.cssText = "cursor: pointer;"
                remove.id = 'nufi'

                var wallet = document.querySelector('#app-nufi-install-wallet');
                wallet.style.cssText = 'display:none;';
            }

            if (key === 'lace') {

                var remove = document.querySelector('#js-connect-lace');
                remove.style.cssText = "cursor: pointer;"
                remove.id = 'lace'

                var wallet = document.querySelector('#app-lace-install-wallet');
                wallet.style.cssText = 'display:none;';
            }

            if (key === 'begin') {

                var remove = document.querySelector('#js-connect-begin');
                remove.style.cssText = "cursor: pointer;"
                remove.id = 'begin'

                var wallet = document.querySelector('#app-begin-install-wallet');
                wallet.style.cssText = 'display:none;';
            }

        }
    }
}



var walletConnection = {};


async function synchWalletInfo() {


    await refreshData();

    //await initTransactionBuilder();

    //console.log(State);
}

function generateScriptAddress() {

    WalletAPI.then((data) => {

        data.getUsedAddresses().then((usedAddresses) => {
            const script = PlutusScript.from_bytes(hex2Bytes(State.plutusScriptCborHex, "hex"))
            const blake2bhash = "67f33146617a5e61936081db3b2117cbf59bd2123748f58ac9678656";
            const scripthash = ScriptHash.from_bytes(hex2Bytes(blake2bhash, "hex"));

            const cred = StakeCredential.from_scripthash(scripthash);
            const networkId = NetworkInfo.testnet().network_id();
            const baseAddr = EnterpriseAddress.new(networkId, cred);
            const addr = baseAddr.to_address();
            const addrBech32 = addr.to_bech32();

            //console.log(buf2Hex(addr.to_bytes(), "utf8").toString("hex"));


            const ScriptAddress = Address.from_bech32("addr_test1wpnlxv2xv9a9ucvnvzqakwepzl9ltx7jzgm53av2e9ncv4sysemm8");
            //console.log(buf2Hex(ScriptAddress.to_bytes(), "utf8").toString("hex"));

            //console.log(ScriptAddress.to_bech32())
            //console.log(addrBech32)

            //console.log(scripthash);
        });

    });
}

function checkIfWalletFound() {
    const walletKey = State.whichWalletSelected;
    const walletFound = !!window?.cardano?.[walletKey];
    State.walletFound = walletFound;
    return walletFound;
}

function checkIfWalletEnabled() {
    let walletIsEnabled = false;

    try {
        const walletName = State.whichWalletSelected;
        walletIsEnabled =  window.cardano[walletName].isEnabled();
    } catch (err) {
        //console.log(err)
    }

    State.walletIsEnabled = walletIsEnabled;

    return walletIsEnabled;

}

async function enableWallet() {
    const walletKey = State.whichWalletSelected;
    try {
        WalletAPI = window.cardano[walletKey].enable();
        //console.log(WalletAPI);
    } catch (err) {
        //console.log(err);
    }
    return checkIfWalletEnabled();
}

function getAPIVersion() {
    const walletKey = State.whichWalletSelected;
    const walletAPIVersion = window?.cardano?.[walletKey].apiVersion;
    const wallletIco = window?.cardano?.[walletKey].ico
    //console.log(wallletIco);
    State.walletAPIVersion = walletAPIVersion;

    return walletAPIVersion;
}

function getWalletName() {
    const walletKey = State.whichWalletSelected;
    const walletName = window?.cardano?.[walletKey].name;
    State.walletName = walletName;
    return walletName;
}

async function getNetworkId() {
    try {

        //console.log(WalletAPI);

        WalletAPI.then((data) => {
            
            data.getNetworkId().then((network) => {
                const networkId = network
                State.networkId = networkId; // 0 = Testnet / 1 = Mainnet
            })
        });

    } catch (err) {
        //console.log(err)
    }
} 



function getUtxos() {


    let Utxos = [];
    let obj = null;
    try {
       
        WalletAPI.then((data) => {


          
            cardano.getUtxos().then((rawUtxos) => {

                //const rawUtxos = Utxos;

                for (const rawUtxo of rawUtxos) {
                    const utxo = TransactionUnspentOutput.from_bytes(hex2Bytes(rawUtxo, "hex"));
                    const input = utxo.input();
                    const txid = buf2Hex(input.transaction_id().to_bytes(), "utf8").toString("hex");
                    const txindx = input.index();
                    const output = utxo.output();
                    const amount = output.amount().coin().to_str(); // ADA amount in lovelace
                    const multiasset = output.amount().multiasset();

                    //console.log("ADA:" + amount);

                    var lblAmountWallet = document.querySelector('.js-amount-wallet');

                    var amountPresentation = parseInt(amount) / 1000000;


                    if (lblAmountWallet !== null) {
                        lblAmountWallet.innerHTML = amountPresentation;
                    }
                    

                    let multiAssetStr = "";

                    if (multiasset) {

                        //console.log("MULTIASSET");

                        const keys = multiasset.keys() // policy Ids of thee multiasset

                        const N = keys.len();
                        //console.log(`${N} Multiassets in the UTXO`)


                        for (let i = 0; i < N; i++) {
                            const policyId = keys.get(i);
                            const policyIdHex = buf2Hex(policyId.to_bytes(), "utf8").toString("hex");

                            //console.log('policyId:' + policyIdHex)

                            const assets = multiasset.get(policyId)
                            const assetNames = assets.keys();
                            const K = assetNames.len()
                            //console.log(K + ' Assets in the Multiasset');

                            for (let j = 0; j < K; j++) {
                                const assetName = assetNames.get(j);
                                const assetNameString = buf2Hex(assetName.name(), "utf8").toString();
                                const assetNameHex = buf2Hex(assetName.name(), "utf8").toString("hex");

                                const multiassetAmt = multiasset.assets; //ainda precisa ajustar

                                //console.log(assetNameString);

                                multiAssetStr += multiassetAmt + policyIdHex + '.' + assetNameHex + assetNameString;

                                //console.log(assetNameString);

                                //console.log('Asset Name:' + assetNameHex)
                            }
                        }
                    }


                    obj = {
                        txid: txid,
                        txindx: txindx,
                        amount: amount,
                        str: txid + ' #' + txindx + '=' + amount,
                        multiAssetStr: multiAssetStr,
                        TransactionUnspentOutput: utxo
                    }
                }

                Utxos.push(obj);
                //console.log('utxo:' + obj.str)

            })
        });

        State.Utxos = Utxos;
    } 
    catch (err) {
        console.log(err)
    }
}

function getCollateral() {


    let CollatUtxos = [];

    try {

        let collateral = [];

        const wallet = State.whichWalletSelected;

        if (wallet === "nami") {


            WalletAPI.then((data) => {

                data.experimental.getCollateral().then((coll) => {

                    collateral = coll;
                })
            });

        } else {

            WalletAPI.then((data) => {

                data.getCollateral().then((coll) => {

                    collateral = coll;
                })
            });
  
        }

        for (const x of collateral) {
            const utxo = TransactionUnspentOutput.from_bytes(buf2Hex(x));
            CollatUtxos.push(utxo)
            console.log(utxo)
        }
        State.CollatUtxos = CollatUtxos;


    } catch (err) {
        console.log(err)
    }
}

async function getBalance() {
   
   
    try {
        State.balance = await WalletAPI.then((api) => {
            
            var balanc = api.getBalance().then((bal) => {

                var balanceCBORHex = bal
                const balance = Value.from_bytes(hex2Bytes(balanceCBORHex)).coin().to_str();
                let _balance = balance;
                return _balance;
            });

            return balanc;
        });

    } catch (err) {
        console.log(err)
    }
    
}

function getChangeAddress() {

    try {

        WalletAPI.then((api) => {

            api.getChangeAddress().then((change) => {

                var raw = change
                const changeAddress = convertHexToWalletAddress(raw);
                State.changeAddress = changeAddress;

                //console.log(changeAddress);
            });
        });

    } catch (err) {
        console.log(err)
    }

}

function getRewardAddresses() {

    try {

        WalletAPI.then((api) => {

            api.getRewardAddresses().then((raw) => {

                var rawFirst = raw[0];
                const rewardAddress = convertHexToWalletAddress(rawFirst);
                State.rewardAddress = rewardAddress;

                console.log(rewardAddress);
            });
        });

    } catch (err) {
        console.log(err)
    }

}

async function getUsedAddresses() {

    try {

        State.usedAddress =  WalletAPI.then((api) => {
            
             //api.getUsedAddresses().then(raw => State.usedAddress = convertHexToWalletAddress(raw[0]));
            

            State.usedAddress = cardano.getUsedAddresses().then((raw) => {
                
                var rawFirst = raw[0];
             
                State.usedAddress = convertHexToWalletAddress(rawFirst);
                return State.usedAddress;                
            });

            return State.usedAddress

        });

        return State.usedAddress;
       
    } catch (err) {
        console.log(err)
    }

}

window.refreshData = function()  {

    generateScriptAddress();


    try {
        const walletFound = checkIfWalletFound();
        if (walletFound) {
            getAPIVersion();
            getWalletName();
            const walletEnabled = enableWallet();
            if (walletEnabled) {
                getNetworkId();
                getUtxos();
                getCollateral();
                getBalance();
                getChangeAddress();
                getRewardAddresses();
                getUsedAddresses();
            }
            else {
                State = {
                    Utxos: null,
                    CollatUtxos: null,
                    balance: null,
                    changeAddress: null,
                    rewardAddress: null,
                    usedAddress: null,

                    txBody: null,
                    txBodyCborHex_unsigned: "",
                    txBodyCborHex_signed: "",
                    submittedTxHash: "",
                }
            }
        }

    } catch (err) {
        console.log(err);
    }

}

function initTransactionBuilder() {

    const txBuilder = TransactionBuilder;

    console.log(txBuilder);

    //const txBuilder = TransactionBuilder.new(
    //         TransactionBuilderConfigBuilder.new()
    //        .fee_algo(LinearFee.new(BigNum.from_str(protocolParams.linearFee.minFeeA), BigNum.from_str(protocolParams.linearFee.minFeeB)))
    //        .pool_deposit(BigNum.from_str(protocolParams.poolDeposit))
    //        .key_deposit(BigNum.from_str(protocolParams.keyDeposit))
    //        .coins_per_utxo_word(BigNum.from_str(protocolParams.coinsPerUtxoWord))
    //        .max_value_size(protocolParams.maxValSize)
    //        .max_tx_size(protocolParams.maxTxSize)
    //        .prefer_pure_change(true)
    //        .build()
    //);

    return txBuilder
}

function signTx() {
    //let txVkeyWitnesses = await window.cardano.signTx('84a6008882582057e55f324befd97277a8528ca842f24d9ecf2208e86570f310fb224589eebad501825820c68acf0f7026b3e58d4faf9d52116bdbdb107799663deb5faf2cbc1d6bb592330182582093b0c76a2bd948156dc48ceba6f4484ef34723cc99aca60a3d66f334ca06089201825820379bfe8a37abd2c24422c9f922e43c969f68e07795f2af2b8139c6e87b68f24801825820c73be14eefab1a8d621a147fe4b3ba56ab8e024ac32422858cf2ef01d92465eb01825820fface662f6e8c66111b6fc483f3cf1a9591b677c614e24b6e2d0879a71caf8590182582088635188d4d3898d37665e9fb630e73ba5e0b4159d0aee747e6bb04ae3bdac6a0282582088635188d4d3898d37665e9fb630e73ba5e0b4159d0aee747e6bb04ae3bdac6a010183825839006ba67c57a8b9a3b31e0647e048139850ad1bb79710540950f46529a568c6a084b80ad678a9a912963303b9582dc12dfe689521ee372b99d51a02faf08082583900adbd8754cba85f059a806d6c181d9ca371e011f83f09882ff4a4655893c5f7f5925dbaa03273fba37611516b09381e4fd8a9d1522af8c85c821a001e8480a1581c39023982511dc1b10c43bd3802cef68949179e6303e67669565885d2a14a4e4654202332333334350182583900adbd8754cba85f059a806d6c181d9ca371e011f83f09882ff4a4655893c5f7f5925dbaa03273fba37611516b09381e4fd8a9d1522af8c85c821a1152dd83b3581c00ff24149b4b9aa5004e2fa225f8bc6b2cf7f4322d43fa6a45f88d14a148736473647364736401581c17c11323773853f57ba30c9930c5ff29a60bb84b0f37311bb18e6e8ca8454c5543415301454e4654203101454e4654203201464a756c696e6101484e4f4d45204e465402496473667364667364660154536b656c65746f6e57617272696f7231333635380156536b656c65746f6e57617272696f722023313336353801581c17e49b34623927808d5189bd269518fc40c96e12fd0076551cd3f1cea14973616461736461736401581c1c10d8eeec627fd7bcf142126f3036c605788604a5a92439dedefc7da145414545454501581c39023982511dc1b10c43bd3802cef68949179e6303e67669565885d2a1494e465420233233343501581c51932af73c7375b090e7433a32f891c02d43d7b5cb096d531acc3896a15820537061636573737373737373737373737373737373737373427564202334373601581c585cbd41f126b7c19562cc9a390131450211e7ab16a1db259162de7da148736461736461736401581c636887100d2dacf1841e3ced60e7b010c4b5b70b9d3038b40a7be004a148546573617365746501581c63f2715f8ec09d38e1d686f8ba3839d980c7356c0778a30c3f636e00a1476173646173646118ea581c6668d6da306398aa4b53e3c08c532694558db34f4b56a45f58bf1806a14341414101581c8ffb1c178f9f8ce306b82b7877a45954b4989b41ad51ef6dbd787344a1445641414901581ca5ae15311ed208e7018f5ce77e1436d5e1705b17e32c7164b277751fa1581d537061636573737373737373737373737373737373427564202334373601581ca6019a837f145f8adb62756c8830ba7411f694aa912df78b26a1a544a1457364617364187a581cb3bd545a4426301c68e626aebd1ebfea8c1883839ae56c9baa91bc12a142737301581cb4164fe9a32824060b97793ea01dafe79ed994959c8a4c84714e2463a14a424f54535a233233323301581cb544677085d582fb673986c6aaf77568fad13fa830cc3ffdb40dce76a14353535301581cbfdde4cb703bcc99d8dc7656dd1c2332ea326e70fd24d5ae8a02b8fea145544553544501581cc01e126e79c8c24d0351fa662fb713ad9f73c2805ff4aeda20e25ee1a1581f426f74737a737373737373737373737373737373737373737320233233393401581ce37067ba24e7a835154d28d858bef4a4f3d75d2517063d0a5585e2b4a145464646464601021a0003b555031a00d2a388075820cdbeeba857a07691b312ba1af4c3f3b50bea068b269c17cbde4829d9ff431ab209a1581c39023982511dc1b10c43bd3802cef68949179e6303e67669565885d2a14a4e46542023323333343501a101818201818200581c7af832bd1d295cb6bd9e5eb4586872a9a58d3284af6a97d4892f0a06f582a11902d1a178383339303233393832353131646331623130633433626433383032636566363839343931373965363330336536373636393536353838356432a16a4e465420233233333435a5646e616d656a4e46542023323333343565696d6167657835697066733a2f2f516d54584873344e7577654e4633626a737a3731426565664d487665463469724a4c4579674c525938695a583865667261726974796764736164737361696d656469615479706569696d6167652f706e676973657269616c4e756d6b534f44647361647373613180', true);
    //console.log(txVkeyWitnesses)


    //let submit = await window.cardano.submitTx('84a600828258204505cc24688ea81519f1df5283fedd18f85b5ecfa370c8da79921d35cd2ab42e008258205f67d22fe620898f1c7942d8881c0186f512238caa15286800de2f4ffb949470020183825839006ba67c57a8b9a3b31e0647e048139850ad1bb79710540950f46529a568c6a084b80ad678a9a912963303b9582dc12dfe689521ee372b99d51a02faf08082583900366d0f0bbd3ed4040403bcf1e828e06df5a5e3ec4090b3a133e48fdd734f6bc14bf1ebaad1223b8b188466e3a65bab18aed5279d54085a6f821a001e8480a1581c39023982511dc1b10c43bd3802cef68949179e6303e67669565885d2a14574657374650182583900366d0f0bbd3ed4040403bcf1e828e06df5a5e3ec4090b3a133e48fdd734f6bc14bf1ebaad1223b8b188466e3a65bab18aed5279d54085a6f1a0ba8af06021a00030815031a00d1e2d7075820fa95c0c22bea6da1d725c57aa38c8fae7535058ee0583e4913989625542ac4f509a1581c39023982511dc1b10c43bd3802cef68949179e6303e67669565885d2a145746573746501a20082825820f5e8415786b2c68d6da541929e4eabc18b68be1b3135197fafa24545a9a5f2f0584026e3b7c340fcb9ed271d11070547225f1a6cd909e86cd3bc76927a64bbf1660dea59015f696c419bab4c57c1258efc620204019710ae04c8b11bc9b4077cf00f825820fe93006d573951b0904f1a7af98955bea10f3f4d75667fd72f3efbe1d02697df5840e8b8f865a4f4f6943041d84457800a6c494b196529c24197626eb7c2cf8a97a5c6dbd9d2852580fac746cbcdaa8272856a9404dd4ac6c3609bdce0b462a9ff0e01818201818200581c7af832bd1d295cb6bd9e5eb4586872a9a58d3284af6a97d4892f0a06f582a11902d1a178383339303233393832353131646331623130633433626433383032636566363839343931373965363330336536373636393536353838356432a1781a3734363537333734363532303634373336313634373337333631a6646e616d656574657374656566696c657381a363737263783c697066733a2f2f697066733a2f2f516d54584873344e7577654e4633626a737a3731426565664d487665463469724a4c4579674c525938695a583865646e616d656a74657374652049636f6e696d656469615479706569696d6167652f706e6765696d616765783c697066733a2f2f697066733a2f2f516d54584873344e7577654e4633626a737a3731426565664d487665463469724a4c4579674c525938695a583865667261726974796764736164737361696d656469615479706569696d6167652f706e676973657269616c4e756d6b534f44647361647373613180');
    //console.log(submit)
    
}




function defineWalletUse(walletKey) {
    window.localStorage.setItem('walletKey', walletKey);

    connectWallet(walletKey);
}

function getWalletKey(){
    return window.localStorage.getItem('walletKey');
}

function connectWallet(walletkey) {

    WalletAPI = window.cardano[walletkey].enable();

    //console.log(WalletAPI);
}

async function isConnectWallet(walletkey) {

    var walletIsConnected = false;
    //console.log(walletkey);

    if (walletkey != null) {

        
        walletIsConnected = await window.cardano[walletkey].isEnabled().then((data) => {

            
           return data;
            
        });
    }


    return walletIsConnected;
}

function getInfos() {
    return WalletAPI;
}



function convertHexToWalletAddress(hexWallet) {


    var addr = Address.from_bytes(
        hex2Bytes(hexWallet)
    )

    var addr_b32 = addr.to_bech32();


    return addr_b32;
}



