import {Component, ElementRef, HostListener, OnInit, ViewChild} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {MojConfig} from "../MojConfig";
import {Router} from "@angular/router";

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  praksePodaci:any;
  podaciFakulteti:any;
  smjestajiPodaci:any;
  stipendijePodaci:any;
  podaciUniverziteti:any;

  constructor(private httpKlijent:HttpClient, private router:Router) { }

  ngOnInit(): void {
    this.getPrakse();
    this.getSmjestaji();
    this.getStipendije();
  }

  getPrakse() {
    this.httpKlijent.get(MojConfig.adresa_servera + "/Praksa/GetAll").subscribe(((x: any) => {
      this.praksePodaci = x;
    }));
  }

  getFakultete() {
    this.httpKlijent.get(MojConfig.adresa_servera + "/Fakultet/GetAll").subscribe(((x: any) => {
      this.podaciFakulteti = x;
    }));
  }

  private getSmjestaji() {
    this.httpKlijent.get(MojConfig.adresa_servera + "/Smjestaj/Get").subscribe(((x: any) => {
      this.smjestajiPodaci = x;
    }));
  }

  private getStipendije() {
    this.httpKlijent.get(MojConfig.adresa_servera + "/Stipendija/GetAll").subscribe(((x: any) => {
      this.stipendijePodaci = x;
    }));
  }

  getUniverziteti() {
    this.httpKlijent.get(MojConfig.adresa_servera + "/Univerzitet/GetAll").subscribe(((x: any) => {
      this.podaciUniverziteti = x;
    }));
  }


  praksaDetalji(praksa: any) {
    this.router.navigate(["praksa-detalji",praksa.id]);
  }

  stipendijaDetlaji(stipendija: any) {
    this.router.navigate(["stipendija-detalji",stipendija.id]);
  }

  smjestajDetlaji(smjestaj: any) {
    this.router.navigate(["smjestaj-detalji",smjestaj.id]);
  }
}
