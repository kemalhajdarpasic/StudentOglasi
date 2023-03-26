import { Component, OnInit } from '@angular/core';
import {MojConfig} from "../MojConfig";
import {HttpClient} from "@angular/common/http";
import {Router} from "@angular/router";

@Component({
  selector: 'app-registracija',
  templateUrl: './registracija.component.html',
  styleUrls: ['./registracija.component.css']
})
export class RegistracijaComponent implements OnInit {
  password: any;
  username: any;
  slika:any;
  slikaURL:string="assets/Images/User%20icon.png";
  tipKorisnika: any;
  broj_indeksa: any;
  ime: any;
  prezime: any;
  fakultetiPodaci: any;
  fakultetID:any=0;
  godinaStudija: any;
  nacin_studiranja: any;
  email: any;
  broj_telefona: any;
  univerzitetiPodaci: any;
  univerzitetID:any=0;
  firmePodaci:any;
  firmaID:any=0;
  pozicija: any;
  stipenditoriPodaci: any;
  stipenditorID:any=0;

  constructor(private httpKlijent:HttpClient, private router: Router) { }

  ngOnInit(): void {
    this.getFakulteti();
    this.getUniverziteti();
    this.getFirme()
    this.getStipenditori();
  }

  chooseFile(files: any) {
    this.slika = files[0];

    const reader = new FileReader();
    reader.onload = () => {
      this.slikaURL = reader.result as string;
    }
    reader.readAsDataURL(this.slika)
  }

  getUniverziteti() {
    this.httpKlijent.get(MojConfig.adresa_servera + "/Univerzitet/GetAll").subscribe(((x: any) => {
      this.univerzitetiPodaci = x;
    }));
  }

  getFirme() {
    this.httpKlijent.get(MojConfig.adresa_servera + "/F/GetFirme").subscribe(((x: any) => {
      this.firmePodaci = x;
    }));
  }

  getFakulteti() {
    this.httpKlijent.get(MojConfig.adresa_servera + "/F/GetAll").subscribe(((x: any) => {
      this.fakultetiPodaci = x;
    }));
  }

  getStipenditori() {
    this.httpKlijent.get(MojConfig.adresa_servera + "/F/GetStipenditori").subscribe(((x: any) => {
      this.stipenditoriPodaci = x;
    }));
  }

  validacija() {
    return undefined;
  }

  snimi() {
    if (this.tipKorisnika == 'student') {
      this.snimiStudenta();
    }
      if (this.tipKorisnika == 'izdavac') {
        this.snimiIzdavaca();
    }
    if (this.tipKorisnika == 'referentFakulteta') {
      this.snimireferentFakulteta();
    }
    if (this.tipKorisnika == 'referentUniverziteta') {
      this.snimireferentUniverziteta();
    }
    if (this.tipKorisnika == 'uposlenikFirme') {
      this.snimiuposlenikFirme();
    }
    if (this.tipKorisnika == 'uposlenikStipenditora') {
      this.snimiuposlenikStipenditora();
    }
  }

  private snimiStudenta() {
    const formData = new FormData();
    formData.append('id', '0');
    formData.append('slika', this.slika);
    formData.append('password', this.password);
    formData.append('username', this.username);
    formData.append('ime', this.ime);
    formData.append('prezime', this.prezime);
    formData.append('broj_indeksa', this.broj_indeksa);
    formData.append('godinaStudija', this.godinaStudija);
    formData.append('nacin_studiranja', this.nacin_studiranja);
    formData.append('fakultetID', this.fakultetID);

    this.httpKlijent.post(MojConfig.adresa_servera + "/Student/Snimi", formData).subscribe((s: any) => {
      this.router.navigateByUrl("/");
    })
  }

  private snimiIzdavaca() {
    const formData = new FormData();
    formData.append('id', '0');
    formData.append('slika', this.slika);
    formData.append('password', this.password);
    formData.append('username', this.username);
    formData.append('ime', this.ime);
    formData.append('prezime', this.prezime);
    formData.append('email', this.email);
    formData.append('broj_telefona', this.broj_telefona);
    this.httpKlijent.post(MojConfig.adresa_servera+"/IzdavacSmjestaja/Snimi",formData).subscribe((x:any)=>{
      this.router.navigateByUrl("/");
    })
  }

  private snimireferentFakulteta() {
    const formData = new FormData();
    formData.append('id', '0');
    formData.append('slika', this.slika);
    formData.append('password', this.password);
    formData.append('username', this.username);
    formData.append('ime', this.ime);
    formData.append('prezime', this.prezime);
    formData.append('email', this.email);
    formData.append('fakultetID', this.fakultetID);
    this.httpKlijent.post(MojConfig.adresa_servera+"/ReferentFakulteta/Snimi",formData).subscribe((x:any)=>{
      this.router.navigateByUrl("/");
    })
  }

  private snimireferentUniverziteta() {
    const formData = new FormData();
    formData.append('id', '0');
    formData.append('slika', this.slika);
    formData.append('password', this.password);
    formData.append('username', this.username);
    formData.append('ime', this.ime);
    formData.append('prezime', this.prezime);
    formData.append('email', this.email);
    formData.append('univerzitetID', this.univerzitetID);
    this.httpKlijent.post(MojConfig.adresa_servera+"/ReferentUniverziteta/Snimi",formData).subscribe((x:any)=>{
      this.router.navigateByUrl("/");
    })
  }

  private snimiuposlenikFirme() {
    const formData = new FormData();
    formData.append('id', '0');
    formData.append('slika', this.slika);
    formData.append('password', this.password);
    formData.append('username', this.username);
    formData.append('ime', this.ime);
    formData.append('prezime', this.prezime);
    formData.append('email', this.email);
    formData.append('firmaID', this.firmaID);
    formData.append('pozicija', this.pozicija);
    this.httpKlijent.post(MojConfig.adresa_servera+"/UposlenikFirme/Snimi",formData).subscribe((x:any)=>{
      this.router.navigateByUrl("/");
    })
  }

  private snimiuposlenikStipenditora() {
    const formData = new FormData();
    formData.append('id', '0');
    formData.append('slika', this.slika);
    formData.append('password', this.password);
    formData.append('username', this.username);
    formData.append('ime', this.ime);
    formData.append('prezime', this.prezime);
    formData.append('email', this.email);
    formData.append('stipenditorID', this.stipenditorID);
    this.httpKlijent.post(MojConfig.adresa_servera+"/UposlenikStipenditora/Snimi",formData).subscribe((x:any)=>{
      this.router.navigateByUrl("/");
    })
  }
}
