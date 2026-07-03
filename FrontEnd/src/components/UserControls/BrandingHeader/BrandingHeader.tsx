import React from "react";
import "./brandingheader.css";
import appIcon from "../../../assets/app-icons/recipes-icon.png";

function BrandingHeader() {
    return (
        <div className="branding-header">
            <div className="branding-header-content">

                <div className="branding-header-left">
                    <img
                        src={appIcon}
                        alt="App Icon"
                        className="branding-header-icon"
                    />
                </div>

                <div className="branding-header-text">
                    <span className="branding-header-title"><span className="branding-header-M">M</span> Recipes</span>
                    <span className="branding-header-tagline">
                        Organize, create, and manage your favorite recipes.
                    </span>
                </div>

            </div>
        </div>
    );
}

export default BrandingHeader;
