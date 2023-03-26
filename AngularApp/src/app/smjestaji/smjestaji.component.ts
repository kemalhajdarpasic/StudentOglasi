import { Component, OnInit } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {MatDialog} from "@angular/material/dialog";
import {MojConfig} from "../MojConfig";
import {formatDate} from "@angular/common";
import {LoginInformacije} from "../helper/login-informacije";
import {AutentifikacijaHelper} from "../helper/autentifikacija-helper";

@Component({
  selector: 'app-smjestaji',
  templateUrl: './smjestaji.component.html',
  styleUrls: ['./smjestaji.component.css']
})
export class SmjestajiComponent implements OnInit {

  displayedColumns: string[] = ['naslov', 'ime_izdavaca', 'cijena', 'brojSoba','kapacitet','parking','grad_naziv','akcije'];
  smjestajiPodaci: any;
  dialogtitle: any;
  smjestaj:any;
  slika:any;
  slikaURL:string="assets/Images/no-image.jpg";
  gradoviPodaci: any;
  izdavaciPodaci: any;
  filterGrad: any;
  filterIzdavac: any;
  korisnik: any;

  constructor(private httpKlijent:HttpClient,private dialog: MatDialog) { }

  ngOnInit(): void {
    this.korisnik=this.loginInfo().autentifikacijaToken?.korisnik;
    this.getSmjestaji();
    this.getGradovi();

    if(this.korisnik.isAdmin)
      this.getIzdavaci();
  }

  loginInfo():LoginInformacije {
    return AutentifikacijaHelper.getLoginInfo();
  }

  openDialog(templateRef:any) {
    this.dialog.open(templateRef, {
      width:'60%'
    });
  }

  private getSmjestaji() {

    if(this.korisnik.isIzdavacSmjestaja) {
      this.httpKlijent.get(MojConfig.adresa_servera + "/Smjestaj/Get?izdavacID=" + this.korisnik.id).subscribe(((x: any) => {
        this.smjestajiPodaci = x;
      }));
    }

    else {
      this.httpKlijent.get(MojConfig.adresa_servera + "/Smjestaj/Get").subscribe(((x: any) => {
        this.smjestajiPodaci = x;
      }));
    }
  }

  private getGradovi() {
    this.httpKlijent.get(MojConfig.adresa_servera + "/F/GetGradovi").subscribe(((x: any) => {
      this.gradoviPodaci = x;
    }));
  }

  chooseFile(files: any) {
    this.slika = files[0];

    const reader = new FileReader();
    reader.onload = () => {
      this.slikaURL = reader.result as string;
    }
    reader.readAsDataURL(this.slika)
  }

  dodajOglas() {
    this.dialogtitle='Dodaj oglas';
    this.dialogtitle='Dodaj objavu';
    this.slikaURL="assets/Images/no-image.jpg";
    this.smjestaj={
      id:0,
      naslov:'',
      rokPrijave:'',
      opis:'',
      cijena:'',
      kapacitet:'',
      dodatneUsluge:'',
      brojSoba:'',
      parking:false,
      nacinGrijanja:'',
      gradID:0,
      izdavacID: this.korisnik.isIzdavacSmjestaja? this.korisnik.id : 0
    }
  }
  validacija() {
    return this.smjestaj.rokPrijave==''||
      this.smjestaj.naslov==''||
      this.smjestaj.opis==''||
      this.smjestaj.uslovi==''||
      this.smjestaj.cijena==''||
      this.smjestaj.kapacitet==''||
      this.smjestaj.dodatneUsluge==''||
      this.smjestaj.brojSoba==''||
      this.smjestaj.nacinGrijanja==''||
      this.slikaURL=='assets/Images/no-image.jpg'||
      this.smjestaj.gradID==''||
      this.smjestaj.izdavacID==0;
  }

  snimi() {
    const formData=new FormData();
    formData.append('id',this.smjestaj.id);
    formData.append('naslov',this.smjestaj.naslov);
    formData.append('rokPrijave',formatDate(this.smjestaj.rokPrijave,'shortDate','en-US'));
    formData.append('slika',this.slika);
    formData.append('opis',this.smjestaj.opis);
    formData.append('cijena',this.smjestaj.cijena);
    formData.append('kapacitet',this.smjestaj.kapacitet);
    formData.append('dodatneUsluge',this.smjestaj.dodatneUsluge);
    formData.append('brojSoba',this.smjestaj.brojSoba);
    formData.append('parking',this.smjestaj.parking);
    formData.append('nacinGrijanja',this.smjestaj.nacinGrijanja);
    formData.append('gradID',this.smjestaj.gradID);
    formData.append('izdavacID',this.smjestaj.izdavacID);
    this.httpKlijent.post(MojConfig.adresa_servera+"/Smjestaj/Snimi",formData).subscribe((s:any)=>{
      this.getSmjestaji();
    })
  }

  editOglas(x:any) {
    this.dialogtitle='Edit oglasa';
    this.smjestaj=x;
    this.slikaURL=this.smjestaj.slika;
  }
  obrisiOglas(x:any) {
    this.httpKlijent.post(MojConfig.adresa_servera+"/Smjestaj/Obrisi",x.id).subscribe((s:any)=>{
      this.getSmjestaji();
    })
  }

  private getIzdavaci() {
    this.httpKlijent.get(MojConfig.adresa_servera + "/IzdavacSmjestaja/GetAll").subscribe(((x: any) => {
      this.izdavaciPodaci = x;
    }));
  }

  getPodaci() {
    if(this.smjestajiPodaci==null)
      return [];
    return this.smjestajiPodaci.filter((x:any)=>(
      (this.filterGrad!=null?x.gradID==this.filterGrad:true)&&
      (this.filterIzdavac!=null?x.izdavacID==this.filterIzdavac:true)
    ));
  }

  restart() {
    this.filterIzdavac=null;
    this.filterGrad=null;
  }
}
