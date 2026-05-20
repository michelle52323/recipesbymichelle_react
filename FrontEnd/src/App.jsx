import { Routes, Route, Navigate } from 'react-router-dom';
import SignIn from './components/SignIn/SignIn.jsx';
import Register from './components/Account/Register/Register';
import ThemeSelectorPage from './components/Account/Themes/Themes';
import Profile from './components/Account/Profile/Profile';
import Dashboard from './components/Dashboard/Dashboard.tsx';
import MyRecipes from './components/RecipeConsole/MyRecipes/MyRecipes';
import RecipeInfo from './components/RecipeConsole/RecipeInfo/RecipeInfo';
import Ingredients from './components/RecipeConsole/Ingredients/Ingredients';
import Steps from './components/RecipeConsole/Steps/Steps';
import View from './components/RecipeConsole/View/View';
import Favorites from './components/RecipeConsole/Favorites/Favorites';

import SelectMeasurementSystem from './components/Account/SelectMeasurementSystem/SelectMeasurementSystem';

// import QuizInfo from './components/QuizBuilder/QuizInfo/QuizInfo'
// import Questions from './components/QuizBuilder/Questions/Questions'

import ChangePasswordPage from './components/Account/ChangePassword/ChangePasswordPage.tsx';
import ForgotPassword from './components/Account/ForgotPassword/ForgotPassword';
import ResetPasswordPage from './components/Account/ForgotPassword/ResetPassword';

import Layout from './components/Layout.tsx';
import LoadCheckAuth from './components/LoadCheckAuth/LoadCheckAuth.jsx';
import FormFooterLink from './components/UserControls/FormFooterLink/FormFooterLink';

//Test pages
import Test from './components/Test/Test';
import TestAPI from './components/Test/TestAPI';

function App() {

    return (

        <Routes>

            {/* PUBLIC ROUTES */}
            <Route
                element={
                    <Layout
                        footerSlots={[
                            <FormFooterLink
                                text="Forgot Password?"
                                linkText="Click here to reset password"
                                linkUrl="/account/forgotpassword"
                            />,
                            <FormFooterLink
                                text="Interested in signing up?"
                                linkText="Register for Online Access"
                                linkUrl="/account/register"
                            />
                        ]}
                    />
                }
            >
                <Route index element={<Navigate to="/signin" replace />} />
                <Route path="signin" element={<SignIn />} />
            </Route>

            <Route
                element={
                    <Layout
                        footerSlots={[
                            <FormFooterLink
                                text="Already have an account?"
                                linkText="Sign in here"
                                linkUrl="/signin"
                            />
                        ]}
                    />
                }
            >
                <Route path="account/register" element={<Register />} />
            </Route>

            {/* FORGOT PASSWORD (when you build it) */}
            <Route
                element={
                    <Layout
                        footerSlots={[
                            <FormFooterLink
                                text="Already have an account?"
                                linkText="Sign in here"
                                linkUrl="/signin"
                            />,
                            <FormFooterLink
                                text="Interested in signing up?"
                                linkText="Register for Online Access"
                                linkUrl="/account/register"
                            />
                        ]}
                    />
                }
            >
                <Route index element={<Navigate to="/account/forgotpassword" replace />} />
                <Route path="account/forgotpassword" element={<ForgotPassword />} />
            </Route>






            {/* AUTHENTICATED ROUTES */}
            <Route element={<Layout />}>

                <Route path="dashboard" element={<Dashboard />} />
                <Route path="loadcheckauth" element={<LoadCheckAuth />} />

                <Route path="account/themes" element={<ThemeSelectorPage />} />
                <Route path="account/profile" element={<Profile />} />
                <Route path="account/selectmeasurementsystem" element={<SelectMeasurementSystem />} />

                <Route path="account/changepassword" element={<ChangePasswordPage />} />
                <Route path="account/forgotpassword" element={<ForgotPassword />} />
                <Route path="account/resetpassword" element={<ResetPasswordPage />} />
                <Route path="account/resetpassword/:token" element={<ResetPasswordPage />} />

                <Route path="recipes/myrecipes" element={<MyRecipes />} />

                <Route path="recipes/recipeinfo" element={<RecipeInfo />} />
                <Route path="recipes/recipeinfo/:id" element={<RecipeInfo />} />

                <Route path="recipes/ingredients" element={<Ingredients />} />
                <Route path="recipes/ingredients/:id" element={<Ingredients />} />

                <Route path="recipes/steps" element={<Steps />} />
                <Route path="recipes/steps/:id" element={<Steps />} />

                <Route path="recipes/view" element={<View />} />
                <Route path="recipes/view/:id" element={<View />} />

                <Route path="recipes/favorites" element={<Favorites />} />





            </Route>

            <Route path="test/testAPI" element={<TestAPI />} />
            <Route path="test/test" element={<Test />} />

        </Routes>


    );
}



export default App;



